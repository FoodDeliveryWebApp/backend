-- RatedById and RestaurantId are FKs generated from navigation property names
INSERT INTO stakeholders."RestaurantRatings" ("Id", "Rating", "Comment", "RatedById", "RestaurantId", "CreatedAt", "isDeleted") VALUES
(-1, 8, 'Odlicna pizza, preporucujem!',           -21, -1, '2024-03-02 10:00:00+00', false),
(-2, 6, 'Solidno, ali malo spora dostava.',        -22, -1, '2024-03-03 11:00:00+00', false),
(-3, 9, 'Fantastican kineski, sveze i ukusno.',    -21, -2, '2024-03-04 14:00:00+00', false),
(-4, 5, 'Ocekivao sam vise od kafane.',            -23, -3, '2024-03-05 16:00:00+00', false),
(-5, 7, 'Dobra atmosfera i ukusna hrana.',         -22, -3, '2024-03-06 18:00:00+00', false);
