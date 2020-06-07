SELECT
	count(Ratings.Rating) as TotalRatings, Movies.Genres
FROM	
	Ratings, Movies
WHERE 
	Ratings.MovieID = Movies.MovieId
AND
	Ratings.UserID = 121
GROUP BY
	Movies.Genres