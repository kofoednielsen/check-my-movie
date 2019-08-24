using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CheckMyMovie.Database;
using CheckMyMovie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CheckMyMovie.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        static HttpClient client = new HttpClient();
        const string MOVIE_API_ADDRESS = "http://www.omdbapi.com/?apikey=56cdd64f&";
        private ILogger logger;
        private DatabaseContext context;

        public MoviesController(DatabaseContext context, ILogger<MoviesController> logger)
        {
            context.Database.EnsureCreated();
            this.logger = logger;
            this.context = context;
        }

        [HttpPost("{id}")]
        public async Task Put(string id, [FromBody]bool isChecked)
        {
            try
            {
                var movie = await context.Movies.FindAsync(id);
                movie.Checked = isChecked;
                await context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Failed to save movie check change to db");
            }
        }

        [HttpGet("{idsString}")]
        public async Task<ActionResult<object>> Get(string idsString)
        {
            var ids = idsString.Split(",").Select(s => s.Trim());
            var idsToFetch = new List<string>();
            var movies = new List<Movie>();
            foreach(var id in ids)
            {
                var movie = await context.Movies.FindAsync(id);
                if(movie == null)
                {
                    idsToFetch.Add(id);
                }
                else
                {
                    movie.FromLocalDatabase = true;
                    movies.Add(movie);
                }
            }
            var tasks = idsToFetch.Select(id => fetchMovie(id)).ToArray();
            Task.WaitAll(tasks);

            //Get the valid fetched movies, that returned an ID.
            var fetchedMovies = tasks.Where(t => t.Result.ID != null).Select(t => t.Result);
            await context.AddRangeAsync(fetchedMovies);
            await context.SaveChangesAsync();
            movies.AddRange(fetchedMovies);
            return movies;
        }

        private async Task<Movie> fetchMovie(string id)
        {
            var movie = new Movie();
            try
            {
                var requestUrl = $"{MOVIE_API_ADDRESS}i={id}";
                HttpResponseMessage response = await client.GetAsync(requestUrl);
                var jsonString = response.Content.ReadAsStringAsync().Result;
                response.EnsureSuccessStatusCode();
                movie = JsonConvert.DeserializeObject<Movie>(jsonString);
                return movie;
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Failed to fetch movie from imdb");
                return movie;
            }
        }
    }
}
