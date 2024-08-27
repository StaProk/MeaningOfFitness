CREATE MOFDatabase
GO

CREATE SCHEMA MOFAppSchema
GO 

CREATE TABLE MOFAppSchema.Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50),
	LastName NVARCHAR(50),
	Email NVARCHAR(50),
	Gender NVARCHAR(50),
    SportTitle NVARCHAR(50),
    HeightInCentimeters INT,
    WeightOfPersonInKilos INT,
	Active BIT,
);

CREATE TABLE MOFAppSchema.Meals (
    MealId INT IDENTITY(1,1),
    UserId INT,
    MealTitle NVARCHAR(50),
	MealContent NVARCHAR(MAX),
	GramsOfCarbohydrates INT,
	GramsOfProtein INT,
    GramsOfFats INT,
    GramsOfFibers INT,
    TotalCalories INT,
	Shared BIT,
    MealCreated DATETIME,
    MealUpdated DATETIME
);

CREATE CLUSTERED INDEX cix_Meals_UserId_MealId ON MOFAppSchema.Meals(UserId, MealId)

CREATE TABLE MOFAppSchema.PersonalRecordKiloses (
    PersonalRecordKilosId INT IDENTITY(1,1),
    UserId INT,
    ExerciseTitle NVARCHAR(50),
    WeightInKilos INT,
    NumberOfRepetitions INT,
    Shared BIT,
    PersonalRecordKilosCreated DATETIME,
    PersonalRecordKilosUpdated DATETIME
)

CREATE CLUSTERED INDEX cix_PersonalRecordKiloses_UserId_PersonalRecordKilosId ON MOFAppSchema.PersonalRecordKiloses(UserId, PersonalRecordKilosId)

CREATE TABLE MOFAppSchema.PersonalRecordTimes (
    PersonalRecordTimeId INT IDENTITY(1,1),
    UserId INT,
    ExerciseTitle NVARCHAR(50),
    TimeInSeconds INT,
    Shared BIT,
    PersonalRecordTimeCreated DATETIME,
    PersonalRecordTimeUpdated DATETIME
)

CREATE CLUSTERED INDEX cix_PersonalRecordTimes_UserId_PersonalRecordTimeId ON MOFAppSchema.PersonalRecordTimes(UserId, PersonalRecordTimeId)

CREATE TABLE MOFAppSchema.Supplements (
    SupplementId INT IDENTITY(1,1),
    UserId INT,
    SupplementTitle NVARCHAR(50),
    Shared BIT,
    SupplementCreated DATETIME,
    SupplementUpdated DATETIME
)

CREATE CLUSTERED INDEX cix_Supplements_UserId_SupplementId ON MOFAppSchema.Supplements(UserId, SupplementId)

CREATE TABLE MOFAppSchema.Workouts (
    WorkoutId INT IDENTITY(1,1),
    UserId INT,
    WorkoutTitle NVARCHAR(50),
	WorkoutContent NVARCHAR(MAX),
	Shared BIT,
    WorkoutCreated DATETIME,
    WorkoutUpdated DATETIME
);

CREATE CLUSTERED INDEX cix_Workouts_UserId_WorkoutId ON MOFAppSchema.Workouts(UserId, WorkoutId)

CREATE TABLE MOFAppSchema.Auth(
	Email NVARCHAR(50) PRIMARY KEY,
	PasswordHash VARBINARY(MAX),
	PasswordSalt VARBINARY(MAX)
)
