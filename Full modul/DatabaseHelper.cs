using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Full_modul
{
    class DatabaseHelper
    {
        public class AgeGenderGroup
        {
            public string AgeGroup { get; set; }
            public int MaleCount { get; set; }
            public int FemaleCount { get; set; }
        }

        public class CompanyStatistics
        {
            public int TotalEmployees { get; set; }
            public int[] AgeGroups { get; set; } = new int[12];
            public int[] GenderCounts { get; set; } = new int[2];
            public int[] EducationCounts { get; set; } = new int[4];
            public int[] ExperienceCounts { get; set; } = new int[4];
            public double RetentionRate { get; set; }
        }

        public CompanyStatistics GetCompanyStatistics()
        {
            var stats = new CompanyStatistics();

            string query = @"
WITH WorkerData AS (
    SELECT
        gender,
        DATEDIFF(YEAR, birthdate, GETDATE()) AS age,
        DATEDIFF(YEAR, start_date, GETDATE()) AS experience,
        id_education
    FROM
        [calculator].[dbo].[worker]
    WHERE
        end_date IS NULL
),
AgeGroups AS (
    SELECT
        SUM(CASE WHEN age BETWEEN 18 AND 30 THEN CASE WHEN gender = 'М' THEN 1 ELSE 0 END ELSE 0 END) AS Male_18_30,
        SUM(CASE WHEN age BETWEEN 18 AND 30 THEN CASE WHEN gender = 'Ж' THEN 1 ELSE 0 END ELSE 0 END) AS Female_18_30,
        SUM(CASE WHEN age BETWEEN 31 AND 40 THEN CASE WHEN gender = 'М' THEN 1 ELSE 0 END ELSE 0 END) AS Male_31_40,
        SUM(CASE WHEN age BETWEEN 31 AND 40 THEN CASE WHEN gender = 'Ж' THEN 1 ELSE 0 END ELSE 0 END) AS Female_31_40,
        SUM(CASE WHEN age BETWEEN 41 AND 50 THEN CASE WHEN gender = 'М' THEN 1 ELSE 0 END ELSE 0 END) AS Male_41_50,
        SUM(CASE WHEN age BETWEEN 41 AND 50 THEN CASE WHEN gender = 'Ж' THEN 1 ELSE 0 END ELSE 0 END) AS Female_41_50,
        SUM(CASE WHEN age BETWEEN 51 AND 60 THEN CASE WHEN gender = 'М' THEN 1 ELSE 0 END ELSE 0 END) AS Male_51_60,
        SUM(CASE WHEN age BETWEEN 51 AND 60 THEN CASE WHEN gender = 'Ж' THEN 1 ELSE 0 END ELSE 0 END) AS Female_51_60,
        SUM(CASE WHEN age BETWEEN 61 AND 65 THEN CASE WHEN gender = 'М' THEN 1 ELSE 0 END ELSE 0 END) AS Male_61_65,
        SUM(CASE WHEN age BETWEEN 61 AND 65 THEN CASE WHEN gender = 'Ж' THEN 1 ELSE 0 END ELSE 0 END) AS Female_61_65,
        SUM(CASE WHEN age >= 66 THEN CASE WHEN gender = 'М' THEN 1 ELSE 0 END ELSE 0 END) AS Male_66_plus,
        SUM(CASE WHEN age >= 66 THEN CASE WHEN gender = 'Ж' THEN 1 ELSE 0 END ELSE 0 END) AS Female_66_plus,
        SUM(CASE WHEN gender = 'М' THEN 1 ELSE 0 END) AS MaleCount,
        SUM(CASE WHEN gender = 'Ж' THEN 1 ELSE 0 END) AS FemaleCount
    FROM
        WorkerData
),
EducationCounts AS (
    SELECT
        SUM(CASE WHEN id_education = 1 THEN 1 ELSE 0 END) AS Average,
        SUM(CASE WHEN id_education = 2 THEN 1 ELSE 0 END) AS Special,
        SUM(CASE WHEN id_education = 3 THEN 1 ELSE 0 END) AS IncompleteHigher,
        SUM(CASE WHEN id_education = 4 THEN 1 ELSE 0 END) AS Bachelor
    FROM
        WorkerData
),
ExperienceCounts AS (
    SELECT
        SUM(CASE WHEN experience < 5 THEN 1 ELSE 0 END) AS Experience_Under_5,
        SUM(CASE WHEN experience BETWEEN 6 AND 10 THEN 1 ELSE 0 END) AS Experience_6_10,
        SUM(CASE WHEN experience BETWEEN 11 AND 15 THEN 1 ELSE 0 END) AS Experience_11_15,
        SUM(CASE WHEN experience >= 16 THEN 1 ELSE 0 END) AS Experience_16_plus
    FROM
        WorkerData
),
RetentionRate AS (
    SELECT
        COUNT(*) AS CountAtMaxExperience
    FROM
        WorkerData
    WHERE
        experience = (SELECT MAX(experience) FROM WorkerData)
)

SELECT
    (SELECT COUNT(*) FROM WorkerData) AS TotalEmployees,
    (SELECT Male_18_30 FROM AgeGroups) AS Male_18_30,
    (SELECT Female_18_30 FROM AgeGroups) AS Female_18_30,
    (SELECT Male_31_40 FROM AgeGroups) AS Male_31_40,
    (SELECT Female_31_40 FROM AgeGroups) AS Female_31_40,
	(SELECT Male_41_50 FROM AgeGroups) AS Male_41_50,
    (SELECT Female_41_50 FROM AgeGroups) AS Female_41_50,
    (SELECT Male_51_60 FROM AgeGroups) AS Male_51_60,
    (SELECT Female_51_60 FROM AgeGroups) AS Female_51_60,
    (SELECT Male_61_65 FROM AgeGroups) AS Male_61_65,
    (SELECT Female_61_65 FROM AgeGroups) AS Female_61_65,
    (SELECT Male_66_plus FROM AgeGroups) AS Male_66_plus,
    (SELECT Female_66_plus FROM AgeGroups) AS Female_66_plus,
    (SELECT MaleCount FROM AgeGroups) AS MaleCount,
    (SELECT FemaleCount FROM AgeGroups) AS FemaleCount,
    (SELECT Average FROM EducationCounts) AS Average,
    (SELECT Special FROM EducationCounts) AS Special,
    (SELECT IncompleteHigher FROM EducationCounts) AS IncompleteHigher,
    (SELECT Bachelor FROM EducationCounts) AS Bachelor,
    (SELECT Experience_Under_5 FROM ExperienceCounts) AS Experience_Under_5,
    (SELECT Experience_6_10 FROM ExperienceCounts) AS Experience_6_10,
    (SELECT Experience_11_15 FROM ExperienceCounts) AS Experience_11_15,
    (SELECT Experience_16_plus FROM ExperienceCounts) AS Experience_16_plus,
    (SELECT CountAtMaxExperience FROM RetentionRate) AS CountAtMaxExperience
";

            using (var reader = DatabaseConnection.Instance.ExecuteReader(query))
            {
                if (reader.Read())
                {
                    stats.TotalEmployees = reader.GetInt32(0); // Количество действующих работников
                    stats.AgeGroups[0] = reader.GetInt32(1); // Мужчины 18-30
                    stats.AgeGroups[1] = reader.GetInt32(2); // Женщины 18-30
                    stats.AgeGroups[2] = reader.GetInt32(3); // Мужчины 31-40
                    stats.AgeGroups[3] = reader.GetInt32(4); // Женщины 31-40
                    stats.AgeGroups[4] = reader.GetInt32(5); // Мужчины 41-50
                    stats.AgeGroups[5] = reader.GetInt32(6); // Женщины 41-50
                    stats.AgeGroups[6] = reader.GetInt32(7); // Мужчины 51-60
                    stats.AgeGroups[7] = reader.GetInt32(8); // Женщины 51-60
                    stats.AgeGroups[8] = reader.GetInt32(9); // Мужчины 61-65
                    stats.AgeGroups[9] = reader.GetInt32(10); // Женщины 61-65
                    stats.AgeGroups[10] = reader.GetInt32(11); // Мужчины 66+
                    stats.AgeGroups[11] = reader.GetInt32(12); // Женщины 66+

                    stats.GenderCounts[0] = reader.GetInt32(13); // Всего мужчин
                    stats.GenderCounts[1] = reader.GetInt32(14); // Всего женщин

                    stats.EducationCounts[0] = reader.GetInt32(15); // Среднее
                    stats.EducationCounts[1] = reader.GetInt32(16); // Специальное
                    stats.EducationCounts[2] = reader.GetInt32(17); // Неполное высшее
                    stats.EducationCounts[3] = reader.GetInt32(18); // Высшее

                    stats.ExperienceCounts[0] = reader.GetInt32(19); // Стаж меньше 5
                    stats.ExperienceCounts[1] = reader.GetInt32(20); // Стаж 6-10
                    stats.ExperienceCounts[2] = reader.GetInt32(21); // Стаж 11-15
                    stats.ExperienceCounts[3] = reader.GetInt32(22); // Стаж 16+

                    int countAtMaxExperience = reader.GetInt32(23); // Количество работников с макс. опытом
                    stats.RetentionRate = countAtMaxExperience > 0 ? (double)countAtMaxExperience / stats.TotalEmployees : 0; // Коэффициент удержания
                }
            }
            return stats;
        }
    }
}
