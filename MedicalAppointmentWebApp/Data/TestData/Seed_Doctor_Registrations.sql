DECLARE @DoctorRole INT = 1;

DECLARE @InsertedDoctors TABLE
(
    id UNIQUEIDENTIFIER
);

;WITH DoctorsSeed AS
(
    SELECT *
    FROM (VALUES
    (N'Γιώργος', N'Παπαδόπουλος', 1),
    (N'Μαρία', N'Ιωάννου', 0),
    (N'Νίκος', N'Κωνσταντίνου', 1),
    (N'Ελένη', N'Βασιλείου', 0),
    (N'Δημήτρης', N'Γεωργίου', 1),
    (N'Αναστασία', N'Νικολάου', 0),
    (N'Χρήστος', N'Αθανασίου', 1),
    (N'Κατερίνα', N'Αλεξίου', 0),
    (N'Παναγιώτης', N'Πέτρου', 1),
    (N'Σοφία', N'Δημητρίου', 0),
    (N'Ιωάννης', N'Σταματίου', 1),
    (N'Ιωάννα', N'Αντωνίου', 0),
    (N'Αλέξανδρος', N'Καραγιάννης', 1),
    (N'Βασιλική', N'Χατζή', 0),
    (N'Σπύρος', N'Μιχαηλίδης', 1),
    (N'Χριστίνα', N'Οικονόμου', 0),
    (N'Θανάσης', N'Παπαϊωάννου', 1),
    (N'Μυρτώ', N'Σαμαρά', 0),
    (N'Λεωνίδας', N'Τριανταφύλλου', 1),
    (N'Δανάη', N'Κυριακίδου', 0),
    (N'Ανδρέας', N'Λάμπρου', 1),
    (N'Ιφιγένεια', N'Ζαχαρίου', 0),
    (N'Πέτρος', N'Σπυρόπουλος', 1),
    (N'Αγγελική', N'Μανωλά', 0),
    (N'Κώστας', N'Ράπτης', 1),
    (N'Νικολέτα', N'Δούκα', 0),
    (N'Στέφανος', N'Κορρές', 1),
    (N'Ευαγγελία', N'Φωκά', 0),
    (N'Ηλίας', N'Τσάκος', 1),
    (N'Φωτεινή', N'Μάρκου', 0),
    (N'Μάνος', N'Πολίτης', 1),
    (N'Ζωή', N'Κωστή', 0),
    (N'Αλέκος', N'Χριστοφόρου', 1),
    (N'Άννα', N'Καστρινού', 0),
    (N'Τάσος', N'Παπαγεωργίου', 1)
    ) AS t(first_name, last_name, gender)
),
Numbered AS
(
    SELECT
        ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS rn,
        first_name,
        last_name,
        gender
    FROM DoctorsSeed
)
INSERT INTO [dbo].[users]
(
    id,
    first_name,
    last_name,
    email,
    amka,
    gender,
    phone,
    birth_date,
    password,
    registration_date,
    role,
    refresh_token,
    refresh_token_expiry_time
)
OUTPUT INSERTED.id INTO @InsertedDoctors(id)
SELECT
    NEWID(),
    n.first_name,
    n.last_name,
    CONCAT(N'doctor', RIGHT('00' + CAST(n.rn AS varchar(2)), 2), N'@hospital.gr'),
    NULL,
    n.gender,
    CONCAT(N'6900000', RIGHT('00' + CAST(n.rn AS varchar(2)), 2)),
    NULL,
    N'AQAAAAIAAYagAAAAEN9OxDqYMGW7qIq9QVdzbXp+nja1DES0M3K0sP0/RG0pzxG6olLJsnxBwvmAKWOORw==',
    GETDATE(),
    @DoctorRole,
    NULL,
    NULL
FROM Numbered n;


INSERT INTO [dbo].[doctors]
(
    id,
    image,
    bio,
    specialty,
    registration_status
)
SELECT
    id,
    NULL AS image,
    NULL AS bio,
    4 AS specialty,
    0 AS registration_status
FROM @InsertedDoctors;
