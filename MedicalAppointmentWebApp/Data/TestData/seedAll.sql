SET NOCOUNT ON;

DECLARE @Now DATETIME2 = SYSUTCDATETIME();

------------------------------------------------------------
-- 1) DOCTORS
------------------------------------------------------------
DECLARE @Doctors TABLE
(
    first_name NVARCHAR(100),
    last_name NVARCHAR(100),
    email NVARCHAR(254),
    amka NVARCHAR(11),
    gender INT,
    phone NVARCHAR(50),
    birth_date DATE,
    [password] NVARCHAR(MAX),
    specialty INT,
    license_number NVARCHAR(100),
    bio NVARCHAR(500),
    image NVARCHAR(MAX),
    registration_status INT,
    full_address NVARCHAR(MAX),
    lon FLOAT,
    lat FLOAT,
    duration INT,
    monday_start TIME NULL,
    monday_end TIME NULL,
    tuesday_start TIME NULL,
    tuesday_end TIME NULL,
    wednesday_start TIME NULL,
    wednesday_end TIME NULL,
    thursday_start TIME NULL,
    thursday_end TIME NULL,
    friday_start TIME NULL,
    friday_end TIME NULL,
    saturday_start TIME NULL,
    saturday_end TIME NULL,
    sunday_start TIME NULL,
    sunday_end TIME NULL
);

INSERT INTO @Doctors VALUES
(N'Νικόλαος', N'Παπαδόπουλος', N'nikolaos.papadopoulos@d.gr', N'12345678901', 1, N'6900000001', '1980-05-12', N'Password123!', 3,  N'LIC-001', N'Καρδιολόγος με έμφαση στην προληπτική καρδιολογία.', N'd1m.jpg',  1, N'Λεωφόρος Βασιλίσσης Σοφίας 80, Αθήνα 11528', 23.7546, 37.9806, 30, '09:00', '15:00', '09:00', '15:00', '09:00', '15:00', '09:00', '15:00', '09:00', '14:00', NULL, NULL, NULL, NULL),
(N'Μαρία', N'Γεωργίου', N'maria.georgiou@d.gr', N'12345678902', 2, N'6900000002', '1985-02-18', N'Password123!', 7,  N'LIC-002', N'Δερματολόγος με εμπειρία σε κλινική και αισθητική δερματολογία.', N'd3f.jpg',  1, N'Μητροπόλεως 6, Αθήνα 10563', 23.7331, 37.9753, 20, '10:00', '18:00', '10:00', '18:00', '10:00', '18:00', '10:00', '18:00', '10:00', '16:00', NULL, NULL, NULL, NULL),
(N'Δημήτριος', N'Ιωάννου', N'dimitrios.ioannou@d.gr', N'12345678903', 1, N'6900000003', '1978-07-04', N'Password123!', 33, N'LIC-003', N'Ορθοπεδικός χειρουργός με εξειδίκευση σε αθλητικές κακώσεις.', N'd2m.jpg',  1, N'Λεωφόρος Κηφισίας 7, Αθήνα 11523', 23.7619, 37.9875, 30, '08:30', '14:30', '08:30', '14:30', '08:30', '14:30', '08:30', '14:30', '08:30', '13:30', NULL, NULL, NULL, NULL),
(N'Ελένη', N'Καραγιάννη', N'eleni.karagianni@d.gr', N'12345678904', 2, N'6900000004', '1983-09-21', N'Password123!', 18, N'LIC-004', N'Γυναικολόγος με έμφαση στην προγεννητική φροντίδα.', N'd6f.jpg',  1, N'Πανόρμου 115, Αθήνα 11524', 23.7658, 37.9932, 30, '09:00', '17:00', '09:00', '17:00', '09:00', '17:00', '09:00', '17:00', '09:00', '15:00', NULL, NULL, NULL, NULL),
(N'Γεώργιος', N'Νικολαΐδης', N'georgios.nikolaidis@d.gr', N'12345678905', 1, N'6900000005', '1976-11-09', N'Password123!', 44, N'LIC-005', N'Πνευμονολόγος με εμπειρία σε χρόνιες αναπνευστικές παθήσεις.', N'd4m.jpg',  1, N'Πατησίων 123, Αθήνα 11251', 23.7303, 38.0018, 25, '09:00', '14:00', '09:00', '14:00', '12:00', '19:00', '09:00', '14:00', '09:00', '14:00', NULL, NULL, NULL, NULL),
(N'Σοφία', N'Χριστοδούλου', N'sofia.christodoulou@d.gr', N'12345678906', 2, N'6900000006', '1988-01-30', N'Password123!', 41, N'LIC-006', N'Ψυχίατρος ενηλίκων με εξατομικευμένη προσέγγιση.', N'd13f.jpg',  1, N'Σόλωνος 74, Αθήνα 10680', 23.7399, 37.9836, 45, '11:00', '19:00', '11:00', '19:00', '11:00', '19:00', '11:00', '19:00', '11:00', '17:00', NULL, NULL, NULL, NULL),
(N'Παναγιώτης', N'Αλεξίου', N'panagiotis.alexiou@d.gr', N'12345678907', 1, N'6900000007', '1979-06-15', N'Password123!', 26, N'LIC-007', N'Νευρολόγος με κλινική εμπειρία σε κεφαλαλγίες και νευροπάθειες.', N'd5m.jpg',  1, N'Λεωφόρος Μεσογείων 264, Αθήνα 15562', 23.8139, 37.9994, 30, '09:30', '16:30', '09:30', '16:30', '09:30', '16:30', '09:30', '16:30', '09:30', '14:30', NULL, NULL, NULL, NULL),
(N'Κατερίνα', N'Μαυρίδου', N'katerina.mavridou@d.gr', N'12345678908', 2, N'6900000008', '1984-03-02', N'Password123!', 12, N'LIC-008', N'Ενδοκρινολόγος με εμπειρία σε θυρεοειδή και μεταβολισμό.', N'd15f.jpg',  1, N'Ερμού 18, Αθήνα 10563', 23.7295, 37.9767, 30, '10:00', '17:00', '10:00', '17:00', '10:00', '17:00', '10:00', '17:00', '10:00', '15:00', NULL, NULL, NULL, NULL),
(N'Ιωάννης', N'Βλάχος', N'ioannis.vlachos@d.gr', N'12345678909', 1, N'6900000009', '1981-12-14', N'Password123!', 23, N'LIC-009', N'Παθολόγος με ενδιαφέρον στην πρωτοβάθμια φροντίδα.', N'd7m.jpg',  1, N'Αχαρνών 120, Αθήνα 10434', 23.7319, 37.9920, 20, '08:00', '14:00', '08:00', '14:00', '08:00', '14:00', '08:00', '14:00', '08:00', '13:00', NULL, NULL, NULL, NULL),
(N'Άννα', N'Ράπτη', N'anna.rapti@d.gr', N'12345678910', 2, N'6900000010', '1987-08-26', N'Password123!', 31, N'LIC-010', N'Οφθαλμίατρος με έμφαση σε προληπτικούς ελέγχους.', N'd18f.jpg', 1, N'Λεωφόρος Συγγρού 196, Καλλιθέα 17671', 23.7126, 37.9577, 25, '09:00', '15:00', '09:00', '15:00', '09:00', '15:00', '09:00', '15:00', '09:00', '14:00', NULL, NULL, NULL, NULL),

(N'Θάνος', N'Κωστόπουλος', N'thanos.kostopoulos@d.gr', N'12345678911', 1, N'6900000011', '1977-04-01', N'Password123!', 53, N'LIC-011', N'Ουρολόγος με εμπειρία σε λιθίαση και ανδρική υγεία.', N'd8m.jpg', 1, N'Ηρώων Πολυτεχνείου 91, Πειραιάς 18536', 23.6465, 37.9413, 30, '10:00', '18:00', '10:00', '18:00', '10:00', '18:00', '10:00', '18:00', '10:00', '15:00', NULL, NULL, NULL, NULL),
(N'Χρύσα', N'Λάμπρου', N'chrysa.lamprou@d.gr', N'12345678912', 2, N'6900000012', '1986-10-06', N'Password123!', 47, N'LIC-012', N'Ρευματολόγος με έμφαση στα αυτοάνοσα νοσήματα.', N'd20f.jpg', 1, N'Λεωφόρος Δημάρχου Αγγέλου Μεταξά 24, Γλυφάδα 16674', 23.7515, 37.8646, 30, '09:30', '16:30', '09:30', '16:30', '09:30', '16:30', '09:30', '16:30', '09:30', '14:30', NULL, NULL, NULL, NULL),
(N'Μανώλης', N'Σαρρής', N'manolis.sarris@d.gr', N'12345678913', 1, N'6900000013', '1975-05-20', N'Password123!', 16, N'LIC-013', N'Γενικός χειρουργός με εμπειρία σε λαπαροσκοπικές επεμβάσεις.', N'd9m.jpg', 1, N'Λεωφόρος Κηφισίας 49, Μαρούσι 15123', 23.8008, 38.0437, 40, '08:30', '14:30', '08:30', '14:30', '08:30', '14:30', '08:30', '14:30', '08:30', '13:30', NULL, NULL, NULL, NULL),
(N'Ηρώ', N'Τζίμα', N'iro.tzima@d.gr', N'12345678914', 2, N'6900000014', '1989-07-07', N'Password123!', 42, N'LIC-014', N'Ψυχολόγος με εμπειρία σε άγχος και συμβουλευτική ενηλίκων.', N'd25f.jpg', 1, N'Κολοκοτρώνη 15, Κηφισιά 14562', 23.8082, 38.0746, 50, '12:00', '20:00', '12:00', '20:00', '12:00', '20:00', '12:00', '20:00', '12:00', '17:00', NULL, NULL, NULL, NULL),
(N'Πέτρος', N'Μάρκου', N'petros.markou@d.gr', N'12345678915', 1, N'6900000015', '1982-01-19', N'Password123!', 34, N'LIC-015', N'ΩΡΛ με εμπειρία σε αλλεργίες και παιδιατρικά περιστατικά.', N'd10m.jpg', 1, N'Ανδρέα Παπανδρέου 35, Χαλάνδρι 15232', 23.7981, 38.0217, 25, '10:00', '17:00', '10:00', '17:00', '10:00', '17:00', '10:00', '17:00', '10:00', '14:00', NULL, NULL, NULL, NULL),
(N'Φωτεινή', N'Αποστόλου', N'foteini.apostolou@d.gr', N'12345678916', 2, N'6900000016', '1983-02-11', N'Password123!', 15, N'LIC-016', N'Γαστρεντερολόγος με έμφαση στους προληπτικούς ελέγχους.', N'd27f.jpg', 1, N'Λεωφόρος Μεσογείων 395, Αγία Παρασκευή 15343', 23.8162, 38.0089, 30, '09:00', '16:00', '09:00', '16:00', '09:00', '16:00', '09:00', '16:00', '09:00', '14:00', NULL, NULL, NULL, NULL),
(N'Σταύρος', N'Παπανικολάου', N'stavros.papanikolaou@d.gr', N'12345678917', 1, N'6900000017', '1974-09-29', N'Password123!', 25, N'LIC-017', N'Νεφρολόγος με εμπειρία στην υπέρταση και τη ΧΝΝ.', N'd11m.jpg', 1, N'Εθνικής Αντιστάσεως 62, Περιστέρι 12134', 23.6924, 38.0154, 30, '08:30', '14:30', '08:30', '14:30', '08:30', '14:30', '08:30', '14:30', '08:30', '13:30', NULL, NULL, NULL, NULL),
(N'Βασιλική', N'Δρούγκα', N'vasiliki.drougka@d.gr', N'12345678918', 2, N'6900000018', '1990-06-08', N'Password123!', 37, N'LIC-018', N'Φυσικοθεραπεύτρια με έμφαση στη μυοσκελετική αποκατάσταση.', N'd28f.jpg', 1, N'Θηβών 210, Ίλιον 13122', 23.6997, 38.0314, 30, '09:00', '17:00', '09:00', '17:00', '09:00', '17:00', '09:00', '17:00', '09:00', '15:00', '10:00', '13:00', NULL, NULL),
(N'Χρήστος', N'Ζέρβας', N'christos.zervas@d.gr', N'12345678919', 1, N'6900000019', '1981-03-03', N'Password123!', 6,  N'LIC-019', N'Οδοντίατρος με εμπειρία στην αισθητική οδοντιατρική.', N'd12m.jpg', 1, N'Αγίου Κωνσταντίνου 12, Αιγάλεω 12243', 23.6794, 37.9936, 30, '10:00', '18:00', '10:00', '18:00', '10:00', '18:00', '10:00', '18:00', '10:00', '16:00', NULL, NULL, NULL, NULL),
(N'Δέσποινα', N'Ράλλη', N'despina.ralli@d.gr', N'12345678920', 2, N'6900000020', '1988-12-22', N'Password123!', 9,  N'LIC-020', N'Διαιτολόγος με στόχο την εξατομικευμένη διατροφική υποστήριξη.', N'd31f.jpg', 1, N'Λεωφόρος Ελευθερίου Βενιζέλου 120, Νέα Σμύρνη 17122', 23.7163, 37.9451, 40, '09:00', '16:00', '09:00', '16:00', '09:00', '16:00', '09:00', '16:00', '09:00', '14:00', NULL, NULL, NULL, NULL),

(N'Κωνσταντίνος', N'Αναγνώστου', N'konstantinos.anagnostou@d.gr', N'12345678921', 1, N'6900000021', '1979-09-10', N'Password123!', 3,  N'LIC-021', N'Καρδιολόγος με έμφαση στην καρδιακή ανεπάρκεια.', N'd14m.jpg', 1, N'Εγνατία 90, Θεσσαλονίκη 54624', 22.9444, 40.6342, 30, '09:00', '15:00', '09:00', '15:00', '09:00', '15:00', '09:00', '15:00', '09:00', '14:00', NULL, NULL, NULL, NULL),
(N'Ευαγγελία', N'Παυλίδου', N'evangelia.pavlidou@d.gr', N'12345678922', 2, N'6900000022', '1986-05-17', N'Password123!', 7,  N'LIC-022', N'Δερματολόγος για κλινική και αισθητική δερματολογία.', N'd32f.jpg', 1, N'Τσιμισκή 122, Θεσσαλονίκη 54621', 22.9496, 40.6298, 20, '10:00', '18:00', '10:00', '18:00', '10:00', '18:00', '10:00', '18:00', '10:00', '16:00', NULL, NULL, NULL, NULL),
(N'Αλέξανδρος', N'Φιλιππίδης', N'alexandros.filippidis@d.gr', N'12345678923', 1, N'6900000023', '1980-08-08', N'Password123!', 44, N'LIC-023', N'Πνευμονολόγος με εμπειρία σε άσθμα και ΧΑΠ.', N'd16m.jpg', 1, N'Μητροπόλεως 45, Θεσσαλονίκη 54623', 22.9469, 40.6326, 25, '09:00', '14:00', '09:00', '14:00', '12:00', '19:00', '09:00', '14:00', '09:00', '14:00', NULL, NULL, NULL, NULL),
(N'Γεωργία', N'Κούτρου', N'georgia.koutrou@d.gr', N'12345678924', 2, N'6900000024', '1987-11-11', N'Password123!', 18, N'LIC-024', N'Γυναικολόγος με επίκεντρο την προληπτική ιατρική.', N'd33f.jpg', 1, N'Αγίας Σοφίας 28, Θεσσαλονίκη 54622', 22.9478, 40.6320, 30, '09:00', '17:00', '09:00', '17:00', '09:00', '17:00', '09:00', '17:00', '09:00', '15:00', NULL, NULL, NULL, NULL),
(N'Ιωάννα', N'Μπάκα', N'ioanna.baka@d.gr', N'12345678925', 2, N'6900000025', '1989-01-25', N'Password123!', 42, N'LIC-025', N'Ψυχολόγος με έμφαση στη διαχείριση άγχους.', N'd26g.jpg', 1, N'Βασιλίσσης Όλγας 120, Θεσσαλονίκη 54645', 22.9608, 40.6119, 50, '12:00', '20:00', '12:00', '20:00', '12:00', '20:00', '12:00', '20:00', '12:00', '17:00', NULL, NULL, NULL, NULL),

(N'Ανδρέας', N'Κανελόπουλος', N'andreas.kanelopoulos@d.gr', N'12345678926', 1, N'6900000026', '1978-04-14', N'Password123!', 16, N'LIC-026', N'Γενικός χειρουργός με εμπειρία σε επεμβατική χειρουργική.', N'd17m.jpg', 1, N'Μαιζώνος 110, Πάτρα 26221', 21.7346, 38.2466, 40, '08:30', '14:30', '08:30', '14:30', '08:30', '14:30', '08:30', '14:30', '08:30', '13:30', NULL, NULL, NULL, NULL),
(N'Ράνια', N'Πετράκη', N'rania.petraki@d.gr', N'12345678927', 2, N'6900000027', '1985-06-19', N'Password123!', 12, N'LIC-027', N'Ενδοκρινολόγος με έμφαση στον σακχαρώδη διαβήτη.', N'placeholder-doctor-image.png', 1, N'Κορίνθου 220, Πάτρα 26221', 21.7315, 38.2440, 30, '10:00', '17:00', '10:00', '17:00', '10:00', '17:00', '10:00', '17:00', '10:00', '15:00', NULL, NULL, NULL, NULL),
(N'Μιχάλης', N'Κρητικός', N'michalis.kritikos@d.gr', N'12345678928', 1, N'6900000028', '1977-02-27', N'Password123!', 31, N'LIC-028', N'Οφθαλμίατρος με εμπειρία σε παθήσεις αμφιβληστροειδούς.', N'd19m.jpg', 1, N'25ης Αυγούστου 12, Ηράκλειο 71202', 25.1362, 35.3387, 25, '09:00', '15:00', '09:00', '15:00', '09:00', '15:00', '09:00', '15:00', '09:00', '14:00', NULL, NULL, NULL, NULL),
(N'Ελισάβετ', N'Μανουσάκη', N'elisavet.manousaki@d.gr', N'12345678929', 2, N'6900000029', '1988-09-04', N'Password123!', 41, N'LIC-029', N'Ψυχίατρος ενηλίκων με εμπειρία στην ψυχοφαρμακολογία.', N'placeholder-doctor-image.png', 1, N'Λεωφόρος Κνωσού 44, Ηράκλειο 71306', 25.1549, 35.3275, 45, '11:00', '19:00', '11:00', '19:00', '11:00', '19:00', '11:00', '19:00', '11:00', '17:00', NULL, NULL, NULL, NULL),
(N'Γεώργιος', N'Τσάκαλος', N'georgios.tsakalos@d.gr', N'12345678930', 1, N'6900000030', '1982-12-01', N'Password123!', 53, N'LIC-030', N'Ουρολόγος με εμπειρία σε λειτουργικές παθήσεις ουροποιητικού.', N'd21m.jpg', 1, N'Παπαναστασίου 60, Λάρισα 41222', 22.4190, 39.6363, 30, '10:00', '18:00', '10:00', '18:00', '10:00', '18:00', '10:00', '18:00', '10:00', '15:00', NULL, NULL, NULL, NULL),
(N'Ναταλία', N'Καρά', N'natalia.kara@d.gr', N'12345678931', 2, N'6900000031', '1990-03-16', N'Password123!', 37, N'LIC-031', N'Φυσικοθεραπεύτρια με έμφαση στην αθλητική αποκατάσταση.', N'placeholder-doctor-image.png', 1, N'Ιάσονος 87, Βόλος 38221', 22.9424, 39.3616, 30, '09:00', '17:00', '09:00', '17:00', '09:00', '17:00', '09:00', '17:00', '09:00', '15:00', '10:00', '13:00', NULL, NULL),
(N'Αντώνης', N'Ζώης', N'antonis.zois@d.gr', N'12345678932', 1, N'6900000032', '1976-07-28', N'Password123!', 26, N'LIC-032', N'Νευρολόγος με ενδιαφέρον σε μνήμη και κινητικές διαταραχές.', N'd22m.jpg', 1, N'Ανεξαρτησίας 25, Ιωάννινα 45444', 20.8522, 39.6650, 30, '09:30', '16:30', '09:30', '16:30', '09:30', '16:30', '09:30', '16:30', '09:30', '14:30', NULL, NULL, NULL, NULL),
(N'Χριστίνα', N'Φωτιάδου', N'christina.fotiadou@d.gr', N'12345678933', 2, N'6900000033', '1987-10-12', N'Password123!', 47, N'LIC-033', N'Ρευματολόγος με εμπειρία στην οστεοπόρωση και τις αρθρίτιδες.', N'placeholder-doctor-image.png', 1, N'Ερυθρού Σταυρού 8, Καβάλα 65403', 24.4125, 40.9396, 30, '09:30', '16:30', '09:30', '16:30', '09:30', '16:30', '09:30', '16:30', '09:30', '14:30', NULL, NULL, NULL, NULL);



------------------------------------------------------------
-- Insert users for doctors if not exists
------------------------------------------------------------
INSERT INTO users
(
    id, first_name, last_name, email, amka, gender, phone, birth_date,
    [password], registration_date, role, refresh_token, refresh_token_expiry_time
)
SELECT
    NEWID(),
    d.first_name,
    d.last_name,
    d.email,
    d.amka,
    d.gender,
    d.phone,
    d.birth_date,
    d.[password],
    @Now,
    1,
    NULL,
    NULL
FROM @Doctors d
WHERE NOT EXISTS
(
    SELECT 1
    FROM users u
    WHERE u.email = d.email
);

------------------------------------------------------------
-- Insert doctors if not exists
------------------------------------------------------------
INSERT INTO doctors
(
    id, image, bio, specialty, license_number, registration_status
)
SELECT
    u.id,
    d.image,
    d.bio,
    d.specialty,
    d.license_number,
    d.registration_status
FROM @Doctors d
JOIN users u ON u.email = d.email
LEFT JOIN doctors doc ON doc.id = u.id
WHERE doc.id IS NULL;

------------------------------------------------------------
-- Insert primary practice for each doctor if not exists
------------------------------------------------------------
INSERT INTO practices
(
    id, doctor_id, duration, lon, lat, full_address, phone,
    is_deleted, deleted_time,
    monday_start, monday_end,
    tuesday_start, tuesday_end,
    wednesday_start, wednesday_end,
    thursday_start, thursday_end,
    friday_start, friday_end,
    saturday_start, saturday_end,
    sunday_start, sunday_end,
    registration_status, registration_date
)
SELECT
    NEWID(),
    u.id,
    d.duration,
    d.lon,
    d.lat,
    d.full_address,
    d.phone,
    0,
    NULL,
    d.monday_start, d.monday_end,
    d.tuesday_start, d.tuesday_end,
    d.wednesday_start, d.wednesday_end,
    d.thursday_start, d.thursday_end,
    d.friday_start, d.friday_end,
    d.saturday_start, d.saturday_end,
    d.sunday_start, d.sunday_end,
    d.registration_status,
    @Now
FROM @Doctors d
JOIN users u ON u.email = d.email
LEFT JOIN practices p
    ON p.doctor_id = u.id
   AND p.full_address = d.full_address
WHERE p.id IS NULL;

------------------------------------------------------------
-- Extra second practices for some doctors
------------------------------------------------------------
DECLARE @ExtraPractices TABLE
(
    doctor_email NVARCHAR(254),
    full_address NVARCHAR(MAX),
    lon FLOAT,
    lat FLOAT,
    phone NVARCHAR(50),
    duration INT,
    monday_start TIME NULL,
    monday_end TIME NULL,
    tuesday_start TIME NULL,
    tuesday_end TIME NULL,
    wednesday_start TIME NULL,
    wednesday_end TIME NULL,
    thursday_start TIME NULL,
    thursday_end TIME NULL,
    friday_start TIME NULL,
    friday_end TIME NULL,
    saturday_start TIME NULL,
    saturday_end TIME NULL,
    sunday_start TIME NULL,
    sunday_end TIME NULL,
    registration_status INT
);

INSERT INTO @ExtraPractices VALUES
(N'nikolaos.papadopoulos@d.gr', N'Λεωφόρος Ποσειδώνος 42, Παλαιό Φάληρο 17561', 23.7008, 37.9282, N'6901000001', 30, '16:00', '20:00', NULL, NULL, '16:00', '20:00', NULL, NULL, '16:00', '20:00', '10:00', '13:00', NULL, NULL, 1),
(N'maria.georgiou@d.gr', N'Λεωφόρος Κηφισίας 215, Μαρούσι 15124', 23.8057, 38.0378, N'6901000002', 20, NULL, NULL, '16:00', '20:00', NULL, NULL, '16:00', '20:00', NULL, NULL, '10:00', '14:00', NULL, NULL, 1),
(N'dimitrios.ioannou@d.gr', N'Θησέως 280, Καλλιθέα 17675', 23.7069, 37.9548, N'6901000003', 30, NULL, NULL, NULL, NULL, '16:00', '20:00', NULL, NULL, '16:00', '20:00', '10:00', '13:00', NULL, NULL, 1),
(N'eleni.karagianni@d.gr', N'Λεωφόρος Ελευθερίου Βενιζέλου 180, Νέα Σμύρνη 17121', 23.7182, 37.9501, N'6901000004', 30, '17:30', '20:30', NULL, NULL, '17:30', '20:30', NULL, NULL, '17:30', '20:30', NULL, NULL, NULL, NULL, 1),
(N'sofia.christodoulou@d.gr', N'Κηφισίας 58, Μαρούσι 15125', 23.8025, 38.0449, N'6901000005', 45, NULL, NULL, '10:00', '14:00', NULL, NULL, '10:00', '14:00', NULL, NULL, NULL, NULL, NULL, NULL, 1),
(N'petros.markou@d.gr', N'Αρτέμιδος 3, Παλλήνη 15351', 23.8838, 38.0064, N'6901000006', 25, '16:00', '20:00', NULL, NULL, '16:00', '20:00', NULL, NULL, NULL, NULL, '10:00', '13:00', NULL, NULL, 1),
(N'konstantinos.anagnostou@d.gr', N'Βασιλίσσης Όλγας 185, Θεσσαλονίκη 54646', 22.9629, 40.6088, N'6901000007', 30, NULL, NULL, '16:00', '20:00', NULL, NULL, '16:00', '20:00', NULL, NULL, '10:00', '13:00', NULL, NULL, 1),
(N'evangelia.pavlidou@d.gr', N'Καραολή και Δημητρίου 18, Εύοσμος 56224', 22.9056, 40.6685, N'6901000008', 20, '16:00', '20:00', NULL, NULL, '16:00', '20:00', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1),
(N'andreas.kanelopoulos@d.gr', N'Γούναρη 76, Πάτρα 26225', 21.7340, 38.2460, N'6901000009', 40, NULL, NULL, NULL, NULL, '16:00', '20:00', NULL, NULL, '16:00', '20:00', NULL, NULL, NULL, NULL, 1),
(N'michalis.kritikos@d.gr', N'Λεωφόρος 62 Μαρτύρων 146, Ηράκλειο 71303', 25.1168, 35.3331, N'6901000010', 25, '16:00', '20:00', NULL, NULL, '16:00', '20:00', NULL, NULL, NULL, NULL, '10:00', '13:00', NULL, NULL, 1),
(N'georgios.tsakalos@d.gr', N'Κούμα 12, Λάρισα 41222', 22.4164, 39.6390, N'6901000011', 30, NULL, NULL, '16:00', '20:00', NULL, NULL, '16:00', '20:00', NULL, NULL, NULL, NULL, NULL, NULL, 1),
(N'antonis.zois@d.gr', N'Δωδώνης 21, Ιωάννινα 45221', 20.8495, 39.6632, N'6901000012', 30, '16:00', '20:00', NULL, NULL, '16:00', '20:00', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1);

INSERT INTO practices
(
    id, doctor_id, duration, lon, lat, full_address, phone,
    is_deleted, deleted_time,
    monday_start, monday_end,
    tuesday_start, tuesday_end,
    wednesday_start, wednesday_end,
    thursday_start, thursday_end,
    friday_start, friday_end,
    saturday_start, saturday_end,
    sunday_start, sunday_end,
    registration_status, registration_date
)
SELECT
    NEWID(),
    u.id,
    ep.duration,
    ep.lon,
    ep.lat,
    ep.full_address,
    ep.phone,
    0,
    NULL,
    ep.monday_start, ep.monday_end,
    ep.tuesday_start, ep.tuesday_end,
    ep.wednesday_start, ep.wednesday_end,
    ep.thursday_start, ep.thursday_end,
    ep.friday_start, ep.friday_end,
    ep.saturday_start, ep.saturday_end,
    ep.sunday_start, ep.sunday_end,
    ep.registration_status,
    @Now
FROM @ExtraPractices ep
JOIN users u ON u.email = ep.doctor_email
LEFT JOIN practices p
    ON p.doctor_id = u.id
   AND p.full_address = ep.full_address
WHERE p.id IS NULL;

------------------------------------------------------------
-- 4) RESULT CHECKS
------------------------------------------------------------
SELECT COUNT(*) AS users_count FROM users;
SELECT COUNT(*) AS doctors_count FROM doctors;
SELECT COUNT(*) AS patients_count FROM patients;
SELECT COUNT(*) AS practices_count FROM practices;
SELECT COUNT(*) AS appointments_count FROM appointments;