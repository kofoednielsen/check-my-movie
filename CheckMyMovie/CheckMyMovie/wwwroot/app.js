
document.getElementById("search-button").onclick = function () {
    var xhr = new XMLHttpRequest();
    var id = document.getElementById("id-box").value;
    xhr.open("GET", `api/movies/${id}`, true);
    xhr.onload = function () {
        var data = JSON.parse(xhr.responseText);
        document.getElementById("movie-container").innerHTML = "";
        for (var i = 0; i < data.length; i++)
            document.getElementById("movie-container").innerHTML += renderMovie(data[i]);
    };
    xhr.send();
}

function checkboxClick(id, element) {
    var xhr = new XMLHttpRequest();
    xhr.open("POST", `api/movies/${id}`, true);
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.send(JSON.stringify(element.checked));
}

function renderMovie(movieData) {
    var stars = "";
    for (var i = 0; i < 10; i++) {
        stars += `<span class="fa fa-star ${(i < Math.round(movieData.imdbRating)) ? "checked" : ""}"></span>`;
    }
    return `
<div class="col-md-4"  >
    <div class="card mb-4 box-shadow">
        <img class="card-img-top" data-src="holder.js/100px225?theme=thumb&amp;bg=55595c&amp;fg=eceeef&amp;text=Thumbnail" alt="Thumbnail [100%x225]" style="height: 450px; width: 100%; display: block;" src="${movieData.Poster}" data-holder-rendered="true">
        <div class="card-body">
            <div class="pagination-centered">
            ${stars}
            </div>
            <h5><b>${movieData.Title}</b></h5>
            <p class="card-text">${movieData.Plot}</p>

            <div class="d-flex justify-content-between align-items-center">
                <div class="checkbox">
                    <label><input onClick="checkboxClick('${movieData.imdbID}', this)" type="checkbox" value="" ${movieData.Checked ? "Checked" : ""}> Check</label>
                </div>
                <small>${movieData.FromLocalDatabase ? "source: localdb" : "source: imdb"}</small>
                <small class="text-muted">${movieData.RunTime}</small>
            </div>
        </div>
    </div>
</div>
`;
}