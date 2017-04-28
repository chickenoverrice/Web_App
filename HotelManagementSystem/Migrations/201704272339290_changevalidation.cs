namespace HotelManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changevalidation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reservation",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        checkIn = c.DateTime(nullable: false),
                        checkOut = c.DateTime(nullable: false),
                        guestsInfo = c.String(),
                        bill = c.Double(nullable: false),
                        firstName = c.String(nullable: false),
                        lastName = c.String(nullable: false),
                        email = c.String(),
                        phone = c.String(),
                        address = c.String(),
                        city = c.String(),
                        state = c.String(),
                        zip = c.String(),
                        roomId = c.Int(nullable: false),
                        personId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.RoomType", t => t.roomId, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.personId, cascadeDelete: true)
                .Index(t => t.roomId)
                .Index(t => t.personId);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        firstName = c.String(nullable: false),
                        lastName = c.String(nullable: false),
                        email = c.String(),
                        sessionId = c.String(),
                        address = c.String(),
                        phone = c.String(),
                        city = c.String(),
                        state = c.String(),
                        zip = c.String(),
                        sessionExpiration = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        id = c.Int(nullable: false),
                        member = c.Boolean(nullable: false),
                        password = c.String(),
                        loyaltyNum = c.Int(nullable: false),
                        stays = c.Int(nullable: false),
                        expirationDate = c.DateTime(nullable: false),
                        lastStay = c.DateTime(nullable: false),
                        roomPref = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Person", t => t.id)
                .ForeignKey("dbo.RoomType", t => t.roomPref, cascadeDelete: true)
                .Index(t => t.id)
                .Index(t => t.roomPref);
            
            CreateTable(
                "dbo.RoomType",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        type = c.String(),
                        basePrice = c.Double(nullable: false),
                        maxGuests = c.Int(nullable: false),
                        numberOfRooms = c.Int(nullable: false),
                        description = c.String(),
                        amenities = c.String(),
                        pic = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Staff",
                c => new
                    {
                        id = c.Int(nullable: false),
                        password = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Person", t => t.id)
                .Index(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Staff", "id", "dbo.Person");
            DropForeignKey("dbo.Reservation", "personId", "dbo.Person");
            DropForeignKey("dbo.Reservation", "roomId", "dbo.RoomType");
            DropForeignKey("dbo.Customer", "roomPref", "dbo.RoomType");
            DropForeignKey("dbo.Customer", "id", "dbo.Person");
            DropIndex("dbo.Staff", new[] { "id" });
            DropIndex("dbo.Customer", new[] { "roomPref" });
            DropIndex("dbo.Customer", new[] { "id" });
            DropIndex("dbo.Reservation", new[] { "personId" });
            DropIndex("dbo.Reservation", new[] { "roomId" });
            DropTable("dbo.Staff");
            DropTable("dbo.RoomType");
            DropTable("dbo.Customer");
            DropTable("dbo.Person");
            DropTable("dbo.Reservation");
        }
    }
}
