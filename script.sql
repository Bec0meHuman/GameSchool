USE [ForSchool]
GO
/****** Object:  Table [dbo].[GameScores]    Script Date: 25.06.2025 12:38:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GameScores](
	[ScoreID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[GameName] [nvarchar](100) NOT NULL,
	[Score] [int] NOT NULL,
	[DatePlayed] [datetime] NULL,
	[Difficulty] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ScoreID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Quiz]    Script Date: 25.06.2025 12:38:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Quiz](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QuizQuestion]    Script Date: 25.06.2025 12:38:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuizQuestion](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionText] [nvarchar](max) NOT NULL,
	[Option1] [nvarchar](255) NOT NULL,
	[Option2] [nvarchar](255) NOT NULL,
	[Option3] [nvarchar](255) NOT NULL,
	[Option4] [nvarchar](255) NOT NULL,
	[CorrectAnswerIndex] [int] NOT NULL,
	[QuizID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 25.06.2025 12:38:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[PasswordHash] [nvarchar](256) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Quiz] ON 

INSERT [dbo].[Quiz] ([ID], [Name]) VALUES (1, N'')
INSERT [dbo].[Quiz] ([ID], [Name]) VALUES (2, N'')
SET IDENTITY_INSERT [dbo].[Quiz] OFF
GO
SET IDENTITY_INSERT [dbo].[QuizQuestion] ON 

INSERT [dbo].[QuizQuestion] ([ID], [QuestionText], [Option1], [Option2], [Option3], [Option4], [CorrectAnswerIndex], [QuizID]) VALUES (1, N'Какая столица Португалии?', N'Лиссабон', N'Венесуэла', N'Лондон', N'Прага', 0, 1)
INSERT [dbo].[QuizQuestion] ([ID], [QuestionText], [Option1], [Option2], [Option3], [Option4], [CorrectAnswerIndex], [QuizID]) VALUES (2, N'Какова первая строка знаменитого романа «Моби Дик»?', N'Здравствуй читатель', N'Зови меня Артур', N'Зови меня Измаил', N'Называй меня хозяин, не ошибешся', 2, 1)
INSERT [dbo].[QuizQuestion] ([ID], [QuestionText], [Option1], [Option2], [Option3], [Option4], [CorrectAnswerIndex], [QuizID]) VALUES (3, N'В каком месяце 28 дней?', N'Февраль', N'Март', N'Август', N'Во всех', 3, 1)
INSERT [dbo].[QuizQuestion] ([ID], [QuestionText], [Option1], [Option2], [Option3], [Option4], [CorrectAnswerIndex], [QuizID]) VALUES (4, N'В каком году был выпущен Крестный отец?', N'1972', N'1970', N'1971', N'1973', 0, 1)
INSERT [dbo].[QuizQuestion] ([ID], [QuestionText], [Option1], [Option2], [Option3], [Option4], [CorrectAnswerIndex], [QuizID]) VALUES (5, N'Если вы упали в безвоздушную дыру, проходящую через всю Землю, сколько времени потребуется, чтобы упасть на другую сторону? (В минутах)', N'100', N'30', N'42', N'52', 2, 1)
INSERT [dbo].[QuizQuestion] ([ID], [QuestionText], [Option1], [Option2], [Option3], [Option4], [CorrectAnswerIndex], [QuizID]) VALUES (6, N'Кто является автором сказки «Кот в сапогах»?', N'Шарль Перро', N'Ганс Христиан Андерсен', N'Братья Гримм', N'Эрнст Теодор Гофман', 0, 2)
INSERT [dbo].[QuizQuestion] ([ID], [QuestionText], [Option1], [Option2], [Option3], [Option4], [CorrectAnswerIndex], [QuizID]) VALUES (7, N'Какая страна самая маленькая в мире?', N'Россия', N'Ватикан', N'Америка', N'Австралия', 1, 2)
INSERT [dbo].[QuizQuestion] ([ID], [QuestionText], [Option1], [Option2], [Option3], [Option4], [CorrectAnswerIndex], [QuizID]) VALUES (8, N'На какой машине ездят волки в м/ф «Маша и Медведь»?', N'Патриот', N'УАЗ («буханка»)', N'Волки не умеют водить машину', N'УАЗ («Бобик»)', 1, 2)
INSERT [dbo].[QuizQuestion] ([ID], [QuestionText], [Option1], [Option2], [Option3], [Option4], [CorrectAnswerIndex], [QuizID]) VALUES (9, N'Это морское млекопитающее. У него на верхней губе есть один длинный бивень. Кто это?', N'Акула молот', N'Акула', N'Дельфин', N'Нарвал', 3, 2)
INSERT [dbo].[QuizQuestion] ([ID], [QuestionText], [Option1], [Option2], [Option3], [Option4], [CorrectAnswerIndex], [QuizID]) VALUES (10, N'2+5*(8-5) = ?', N'37', N'21', N'17', N'51', 2, 2)
SET IDENTITY_INSERT [dbo].[QuizQuestion] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [Email], [CreatedAt]) VALUES (1, N'Oleg', N'123', N'oleg@gmail.com', NULL)
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [Email], [CreatedAt]) VALUES (11, N'124125', N'163613', N'125126', NULL)
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [Email], [CreatedAt]) VALUES (12, N'zcvzfjash', N'768921', N'hdfhdfhdf', NULL)
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [Email], [CreatedAt]) VALUES (13, N'jfgkhklghglg', N'12345678', N'hdlgksjdlgs', NULL)
INSERT [dbo].[Users] ([UserID], [Username], [PasswordHash], [Email], [CreatedAt]) VALUES (14, N'Pasha', N'123', N'agsfhjgasj', NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__536C85E4632EBD67]    Script Date: 25.06.2025 12:38:03 ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__A9D10534B0EBC3C1]    Script Date: 25.06.2025 12:38:03 ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[GameScores] ADD  DEFAULT (getdate()) FOR [DatePlayed]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[GameScores]  WITH CHECK ADD  CONSTRAINT [FK__GameScore__UserI__2A4B4B5E] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[GameScores] CHECK CONSTRAINT [FK__GameScore__UserI__2A4B4B5E]
GO
ALTER TABLE [dbo].[QuizQuestion]  WITH CHECK ADD FOREIGN KEY([QuizID])
REFERENCES [dbo].[Quiz] ([ID])
GO
ALTER TABLE [dbo].[QuizQuestion]  WITH CHECK ADD FOREIGN KEY([QuizID])
REFERENCES [dbo].[Quiz] ([ID])
GO
ALTER TABLE [dbo].[QuizQuestion]  WITH CHECK ADD FOREIGN KEY([QuizID])
REFERENCES [dbo].[Quiz] ([ID])
GO
ALTER TABLE [dbo].[QuizQuestion]  WITH CHECK ADD CHECK  (([CorrectAnswerIndex]>=(0) AND [CorrectAnswerIndex]<=(3)))
GO
ALTER TABLE [dbo].[QuizQuestion]  WITH CHECK ADD CHECK  (([CorrectAnswerIndex]>=(0) AND [CorrectAnswerIndex]<=(3)))
GO
ALTER TABLE [dbo].[QuizQuestion]  WITH CHECK ADD CHECK  (([CorrectAnswerIndex]>=(0) AND [CorrectAnswerIndex]<=(3)))
GO
ALTER TABLE [dbo].[QuizQuestion]  WITH CHECK ADD CHECK  (([CorrectAnswerIndex]>=(0) AND [CorrectAnswerIndex]<=(3)))
GO
ALTER TABLE [dbo].[QuizQuestion]  WITH CHECK ADD CHECK  (([CorrectAnswerIndex]>=(0) AND [CorrectAnswerIndex]<=(3)))
GO
ALTER TABLE [dbo].[QuizQuestion]  WITH CHECK ADD CHECK  (([CorrectAnswerIndex]>=(0) AND [CorrectAnswerIndex]<=(3)))
GO
