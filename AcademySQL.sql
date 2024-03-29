DROP DATABASE AcademyProject
GO

CREATE DATABASE AcademyProject
GO

USE AcademyProject
GO

CREATE TABLE Pictures
(
	Id INT IDENTITY PRIMARY KEY,
	PicturePath NVARCHAR(2048) NOT NULL
);
INSERT INTO Pictures VALUES ('/') -- Default picture is blank

CREATE TABLE Users
(
	Id INT IDENTITY PRIMARY KEY,
	Email NVARCHAR(255) NOT NULL,
	PasswordHash NVARCHAR(255),
	FirstName NVARCHAR(255) NOT NULL,
	LastName NVARCHAR(255) NOT NULL,
	Credits INT NOT NULL DEFAULT 0,
	PictureId INT DEFAULT 1,
	CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
	UpdatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
	FOREIGN KEY (PictureId) REFERENCES Pictures(Id)
);

INSERT INTO Users (Email, PasswordHash, FirstName, LastName)
VALUES ('admin@dongdev.com', '$2a$11$7LhQckiXkSskyGk/2jUn6O0/p7Lw/1WFeFWfkglIyfGl2..gotZnm', 'Trinh', 'Dong');

CREATE TABLE Roles
(
	Id INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(255) NOT NULL
);
INSERT INTO Roles VALUES ('Administrators');
INSERT INTO Roles VALUES ('Moderators');
INSERT INTO Roles VALUES ('Lecturers');
INSERT INTO Roles VALUES ('Students');
INSERT INTO Roles VALUES ('Banned');

CREATE TABLE UserRoles
(
	UserId INT NOT NULL,
	RoleId INT NOT NULL,
	PRIMARY KEY(UserId, RoleId),
	FOREIGN KEY (UserId) REFERENCES Users(Id),
	FOREIGN KEY (RoleId) REFERENCES Roles(Id)
);
INSERT INTO UserRoles VALUES (1, 1);

CREATE TABLE Category
(
	Id INT IDENTITY PRIMARY KEY,
	Name NVARCHAR(255) NOT NULL,
	IsDeleted BIT NOT NULL DEFAULT 0 -- 1 = True, 0 = False
);

CREATE TABLE Courses
(
	Id INT IDENTITY PRIMARY KEY,
	LecturerId INT NOT NULL,
	CategoryId INT NOT NULL,
	PictureId INT DEFAULT 1,
	Title NVARCHAR(255) NOT NULL,
	Description NVARCHAR(2048) NOT NULL,
	Credits INT NOT NULL DEFAULT 0,
	CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
	UpdatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
	IsDeleted BIT NOT NULL DEFAULT 0, -- 1 = True, 0 = False
	FOREIGN KEY (PictureId) REFERENCES Pictures(Id),
	FOREIGN KEY (LecturerId) REFERENCES Users(Id),
	FOREIGN KEY (CategoryId) REFERENCES Category(Id)
);

CREATE TABLE Attendance 
(
	CourseId INT NOT NULL,
	UserId INT NOT NULL,
	Credits INT NOT NULL DEFAULT 0,
	CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
	PRIMARY KEY (CourseId, UserId),
	FOREIGN KEY (CourseId) REFERENCES Courses(Id),
	FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE WillLearns
(
	Id INT IDENTITY PRIMARY KEY,
	CourseId INT NOT NULL,
	Content NVARCHAR(2048) NOT NULL,
	FOREIGN KEY (CourseId) REFERENCES Courses(Id)
);

CREATE TABLE Requirements
(
	Id INT IDENTITY PRIMARY KEY,
	CourseId INT NOT NULL,
	Content NVARCHAR(2048) NOT NULL,
	FOREIGN KEY (CourseId) REFERENCES Courses(Id)
);

CREATE TABLE Tracks 
(
	Id INT IDENTITY PRIMARY KEY,
	CourseId INT NOT NULL,
	Title NVARCHAR(255) NOT NULL,
	IsDeleted BIT NOT NULL DEFAULT 0, -- 1 = True, 0 = False
	FOREIGN KEY (CourseId) REFERENCES Courses(Id)
);

CREATE TABLE Steps 
(
	Id INT IDENTITY PRIMARY KEY,
	TrackId INT NOT NULL,
	Title NVARCHAR(255) NOT NULL,
	Duration INT NOT NULL,
	Content NVARCHAR(MAX),
	EmbedLink NVARCHAR(2048),
	IsDeleted BIT NOT NULL DEFAULT 0, -- 1 = True, 0 = False
	FOREIGN KEY (TrackId) REFERENCES Tracks(Id)
);

CREATE TABLE Progresses
(
	StepId INT NOT NULL,
	UserId INT NOT NULL,
	StartedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
	PRIMARY KEY (StepId, UserId),
	FOREIGN KEY (StepId) REFERENCES Steps(Id),
	FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE Exams (
	Id INT IDENTITY PRIMARY KEY,
	CourseId INT NOT NULL,
	Title NVARCHAR(255) NOT NULL,
	ExamDuration INT NOT NULL DEFAULT 300 CHECK(ExamDuration >= 300 AND ExamDuration <= 21600),
	CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
	IsDeleted BIT NOT NULL DEFAULT 0, -- 1 = True, 0 = False
	FOREIGN KEY (CourseId) REFERENCES Courses(Id)
);

CREATE TABLE ExamQuestions (
    Id INT IDENTITY PRIMARY KEY,
	ExamId INT NOT NULL,
    Content NVARCHAR(2048) NOT NULL,
	IsDeleted BIT NOT NULL DEFAULT 0, -- 1 = True, 0 = False
	FOREIGN KEY (ExamId) REFERENCES Exams(Id)
);

CREATE TABLE ExamOptions (
    Id INT IDENTITY PRIMARY KEY,
    QuestionId INT NOT NULL,
    Content NVARCHAR(2048) NOT NULL,
	FOREIGN KEY (QuestionId) REFERENCES ExamQuestions(Id)
);

CREATE TABLE ExamRightOptions (
	QuestionId INT PRIMARY KEY,
	OptionId INT NOT NULL,
	FOREIGN KEY (QuestionId) REFERENCES ExamQuestions(Id),
	FOREIGN KEY (OptionId) REFERENCES ExamOptions(Id)
);

CREATE TABLE ExamUsers (
    Id INT IDENTITY PRIMARY KEY,
    UserId INT NOT NULL,
	ExamId INT NOT NULL,
	NoOfQuestion INT NOT NULL,
	StartedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
	CompletedAt DATETIME,
	NoOfRightOption INT,
	Mark FLOAT NOT NULL DEFAULT 0 CHECK(Mark >=0 AND Mark <=10),
	FOREIGN KEY (UserId) REFERENCES Users(Id),
	FOREIGN KEY (ExamId) REFERENCES Exams(Id)
);

CREATE TABLE ExamDetails (
    Id INT IDENTITY PRIMARY KEY,
	ExamUserId INT NOT NULL,
    QuestionId INT NOT NULL,
    OptionId INT,
	FOREIGN KEY (ExamUserId) REFERENCES ExamUsers(Id),
	FOREIGN KEY (QuestionId) REFERENCES ExamQuestions(Id),
	FOREIGN KEY (OptionId) REFERENCES ExamOptions(Id)
);

CREATE TABLE Certifications (
    UserId INT NOT NULL,
	CourseId INT NOT NULL,
	Mark FLOAT NOT NULL DEFAULT 0 CHECK(Mark >=0 AND Mark <=10),
	CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
	PRIMARY KEY(UserId, CourseId),
	FOREIGN KEY (UserId) REFERENCES Users(Id),
	FOREIGN KEY (CourseId) REFERENCES Courses(Id)
);

CREATE TABLE Blogs
(
	Id INT IDENTITY PRIMARY KEY,
	UserId INT NOT NULL,
	CategoryId INT NOT NULL,
	Title NVARCHAR(255) NOT NULL,
	Content NVARCHAR(MAX) NOT NULL,
	CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
	UpdatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
	IsDeleted BIT NOT NULL DEFAULT 0, -- 1 = True, 0 = False
	PictureId INT DEFAULT 1,
	FOREIGN KEY (PictureId) REFERENCES Pictures(Id),
	FOREIGN KEY (UserId) REFERENCES Users(Id),
	FOREIGN KEY (CategoryId) REFERENCES Category(Id)
);

CREATE TABLE BlogComments
(
	Id INT IDENTITY PRIMARY KEY,
	BlogId INT NOT NULL,
	UserId INT NOT NULL,
	Content NVARCHAR(2048) NOT NULL,
	CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
	UpdatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
	FOREIGN KEY (BlogId) REFERENCES Blogs(Id),
	FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE Questions
(
	Id INT IDENTITY PRIMARY KEY,
	UserId INT NOT NULL,
	CategoryId INT NOT NULL,
	Title NVARCHAR(255) NOT NULL,
	Content NVARCHAR(MAX) NOT NULL,
	CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
	UpdatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
	IsDeleted BIT NOT NULL DEFAULT 0, -- 1 = True, 0 = False
	PictureId INT DEFAULT 1,
	FOREIGN KEY (UserId) REFERENCES Users(Id),
	FOREIGN KEY (CategoryId) REFERENCES Category(Id)
);

CREATE TABLE Answers
(
	Id INT IDENTITY PRIMARY KEY,
	QuestionId INT NOT NULL,
	UserId INT NOT NULL,
	Content NVARCHAR(2048) NOT NULL,
	CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
	UpdatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
	FOREIGN KEY (QuestionId) REFERENCES Questions(Id),
	FOREIGN KEY (UserId) REFERENCES Users(Id)
);
