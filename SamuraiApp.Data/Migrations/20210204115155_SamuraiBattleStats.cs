using Microsoft.EntityFrameworkCore.Migrations;

namespace SamuraiApp.Data.Migrations
{
    public partial class SamuraiBattleStats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE FUNCTION[dbo].[EarliestBattleFoughtBySamurai](@samuraiId int)
                   RETURNS char(30) AS
                   BEGIN
                     DECLARE @ret char(30)
                     SELECT TOP 1 @ret = Name
                     FROM Battles
                     WHERE Battles.Id IN(SELECT BattleId
                                        FROM SamuraiBattle
                                       WHERE SamuraiId = 2)
                     ORDER BY StartDate
                     RETURN @ret
                   END");

            migrationBuilder.Sql(
                @"CREATE OR ALTER VIEW dbo.SamuraiBattleStats
                  AS
                  SELECT dbo.SamuraiBattle.SamuraiId, dbo.Samurais.Name,
                  COUNT(dbo.SamuraiBattle.BattleId) AS NumberOfBattles,
                          dbo.EarliestBattleFoughtBySamurai(MIN(dbo.Samurais.Id)) AS EarliestBattle
                  FROM dbo.SamuraiBattle INNER JOIN
                       dbo.Samurais ON dbo.SamuraiBattle.SamuraiId = dbo.Samurais.Id
                  GROUP BY dbo.Samurais.Name, dbo.SamuraiBattle.SamuraiId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW dbo.SamuraiBattleStats");
            migrationBuilder.Sql("DROP FUNCTION dbo.EarliestBattleFoughtBySamurai");
        }
    }
}
