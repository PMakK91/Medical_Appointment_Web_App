SET NOCOUNT ON;

DECLARE @i INT = 1;

WHILE @i <= 30
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @UserId UNIQUEIDENTIFIER = NEWID();
        DECLARE @Practice1Id UNIQUEIDENTIFIER = NEWID();
        DECLARE @Practice2Id UNIQUEIDENTIFIER = NEWID();

        -- Keep AMKA short/numeric to avoid truncation.
        -- Produces 11 digits like 10000000001, 10000000002, ...
        DECLARE @Amka NVARCHAR(20) = CAST(10000000000 + @i AS NVARCHAR(20));

        INSERT INTO users
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
        VALUES
        (
            @UserId,
            CONCAT(N'Doctor', @i),
            CONCAT(N'Test', @i),
            CONCAT('doctor', @i, '@example.com'),
            @Amka,
            CASE WHEN @i % 2 = 0 THEN 1 ELSE 2 END,
            CONCAT('6900000', RIGHT('000' + CAST(@i AS VARCHAR(3)), 3)),
            DATEFROMPARTS(1980 + (@i % 15), ((@i - 1) % 12) + 1, ((@i - 1) % 28) + 1),
            N'hashedpassword',
            GETDATE(),
            2, -- doctor role
            NULL,
            NULL
        );

        INSERT INTO doctors
        (
            id,
            image,
            bio,
            specialty,
            registration_status,
            license_number
        )
        VALUES
        (
            @UserId,
            NULL,
            CONCAT(N'Bio for doctor ', @i),
            ((@i - 1) % 8) + 1,
            1,
            CONCAT('LIC', RIGHT('00000' + CAST(@i AS VARCHAR(5)), 5))
        );

        INSERT INTO practices
        (
            id,
            doctor_id,
            full_address,
            registration_status,
            duration,
            friday_end,
            friday_start,
            monday_end,
            monday_start,
            phone,
            saturday_end,
            saturday_start,
            sunday_end,
            sunday_start,
            thursday_end,
            thursday_start,
            tuesday_end,
            tuesday_start,
            wednesday_end,
            wednesday_start,
            lat,
            lon,
            registration_date
        )
        VALUES
        (
            @Practice1Id,
            @UserId,
            CONCAT(N'Practice 1, Street ', @i, N', Athens'),
            1,
            30,
            '17:00', '09:00',
            '17:00', '09:00',
            CONCAT('210555', RIGHT('000' + CAST(@i AS VARCHAR(3)), 3)),
            '14:00', '10:00',
            NULL, NULL,
            '17:00', '09:00',
            '17:00', '09:00',
            '17:00', '09:00',
            37.9838 + (@i * 0.001),
            23.7275 + (@i * 0.001),
            GETDATE()
        );

        INSERT INTO practices
        (
            id,
            doctor_id,
            full_address,
            registration_status,
            duration,
            friday_end,
            friday_start,
            monday_end,
            monday_start,
            phone,
            saturday_end,
            saturday_start,
            sunday_end,
            sunday_start,
            thursday_end,
            thursday_start,
            tuesday_end,
            tuesday_start,
            wednesday_end,
            wednesday_start,
            lat,
            lon,
            registration_date
        )
        VALUES
        (
            @Practice2Id,
            @UserId,
            CONCAT(N'Practice 2, Avenue ', @i, N', Thessaloniki'),
            1,
            45,
            '18:00', '10:00',
            '18:00', '10:00',
            CONCAT('231055', RIGHT('000' + CAST(@i AS VARCHAR(3)), 3)),
            NULL, NULL,
            NULL, NULL,
            '18:00', '10:00',
            '18:00', '10:00',
            '18:00', '10:00',
            40.6401 + (@i * 0.001),
            22.9444 + (@i * 0.001),
            GETDATE()
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        PRINT 'Failed at row ' + CAST(@i AS VARCHAR(10)) + ': ' + ERROR_MESSAGE();
    END CATCH;

    SET @i += 1;
END;