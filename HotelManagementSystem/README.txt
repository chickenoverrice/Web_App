This project uses Model-first approach. Please follow the steps to build the application.

1. Database generation:
   a. Preserve data annotations if there are any.
   b. Use 'HotelDatabase.edmx' file to generate the database.
   c. Add the following two lines to 'HotelDatabase.Context.cs' file:

	public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }

2. Add room types:
   Execute the following SQL commands in RoomTypes Table:

SET IDENTITY_INSERT [dbo].[RoomTypes] ON
INSERT INTO [dbo].[RoomTypes] ([Id], [type], [basePrice], [maxGuests], [numberOfRooms], [description], [amenities], [picture]) VALUES (5002, N'Double Room', 200, 2, 50, N'Refresh in the stylish marble bathroom, with shower, and experience the comfort of a king Hilton Serenity bed, complete with luxurious, allergen-free down comforter and pillows, elegantly striped 250 thread count sheets and duvet cover, plus a mattress set custom designed for added support and comfort. Special touches include bathrobes. Any corresponding photo may not reflect the specific accessible room type or room feature.', N'City view
;Flat-screen TV;Air conditioning;Private bathroom;Free WiFi', N'../pics/double.jpg')
INSERT INTO [dbo].[RoomTypes] ([Id], [type], [basePrice], [maxGuests], [numberOfRooms], [description], [amenities], [picture]) VALUES (5003, N'Single Room', 100, 1, 50, N'Offering the latest in technology and ergonomic comfort, this guest room is 100% non-smoking and feature a 50-inch flat-screen HDTV, large work desk with Herman Miller chair, refrigerator, microwave, coffee/teamaker, safe and MP3 clock/music player. ', N'City view
;Flat-screen TV;Air conditioning;Private bathroom;Free WiFi', N'../pics/single.jpg')
INSERT INTO [dbo].[RoomTypes] ([Id], [type], [basePrice], [maxGuests], [numberOfRooms], [description], [amenities], [picture]) VALUES (5004, N'Executive Suite', 300, 4, 10, N'One-bedroom suites are 660 sq. ft. with one king-sized bed, one queen-sized sofa bed and a spacious living room with a breakfast table. The bathroom offers a therapeutic Kohler shower with multiple heads and spa-quality bath amenities. Some bathrooms include a large soaking tub as well. A 55-inch TV and complimentary WiFi are included. Sleeps 4.', N'Ocean view
;Flat-screen TV;Air conditioning;Private bathroom;Free WiFi', N'../pics/suite.jpg')
INSERT INTO [dbo].[RoomTypes] ([Id], [type], [basePrice], [maxGuests], [numberOfRooms], [description], [amenities], [picture]) VALUES (5005, N'Family Room', 400, 4, 5, N'Enjoy a two-room suite perfectly equipped for comfort and convenience. You¡¯ll feel relaxed and taken care of whether you¡¯re here for an extended stay or just a night. With a private bedroom and separate living room with a sleeper-sofa, you''ve got plenty of room for family.', N'City view
;Flat-screen TV;Air conditioning;Private bathroom;Free WiFi;Safe;Double Locking Doors', N'../pics/family.jpg')
SET IDENTITY_INSERT [dbo].[RoomTypes] OFF


3. Add a staff:
   a. Create an entry in People table with email: staff@lxyz.com
	and fill the rest of the columns as you desire.
   b. Put the entry ID from Step.a into People_Staff.
   c. Login as a staff with the following credentials:
	email: staff@lxyz.com
	password: Staff@123