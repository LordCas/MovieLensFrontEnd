SELECT DISTINCT 
	movies.Title, AVG(Ratings.Rating) AS Average_Rating
FROM 
	Ratings, Movies
WHERE 
	Movies.MovieId = Ratings.MovieID
GROUP BY 
	Movies.Title
HAVING
	AVG(Ratings.Rating) > 45
ORDER BY 
	AVG(Ratings.Rating) DESC
