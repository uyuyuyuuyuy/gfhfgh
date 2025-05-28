using UnityEngine;

public class VillagerSkills
{
    public int strength; // Сила для фермы
    public int endurance; // Выносливость для водоочистной станции
    public int technicalSkills; // Технические навыки для генератора

    private const int MaxSkillValue = 10; // Максимальный уровень навыков

    // Конструктор для генерации случайных значений навыков
    public VillagerSkills()
    {
        strength = Random.Range(0, MaxSkillValue + 1); // Сила от 0 до MaxSkillValue
        endurance = Random.Range(0, MaxSkillValue + 1); // Выносливость от 0 до MaxSkillValue
        technicalSkills = Random.Range(0, MaxSkillValue + 1); // Технические навыки от 0 до MaxSkillValue
    }

    // Метод для вывода информации о текущих навыках в консоль
    public void PrintSkills()
    {
        Debug.Log($"Сила: {strength}, Выносливость: {endurance}, Технические навыки: {technicalSkills}");
    }

    // Увеличение навыков в зависимости от типа здания
    public void IncreaseSkill(Building building, Villager villager)
    {
        bool updated = false; // Флаг для проверки обновления навыков

        switch (building.GetBuildingType())
        {
            case Building.BuildingType.Farm:
                updated = IncreaseStrength(villager);
                break;
            case Building.BuildingType.WaterStation:
                updated = IncreaseEndurance(villager);
                break;
            case Building.BuildingType.Generator:
                updated = IncreaseTechSkills(villager);
                break;
        }
    }

    // Увеличение силы
    private bool IncreaseStrength(Villager villager)
    {
        if (strength < MaxSkillValue)
        {
            strength++;
            Debug.Log($"Сила увеличена до {strength}");
            return true;
        }
        return false;
    }

    // Увеличение выносливости
    private bool IncreaseEndurance(Villager villager)
    {
        if (endurance < MaxSkillValue)
        {
            endurance++;
            Debug.Log($"Выносливость увеличена до {endurance}");
            return true;
        }
        return false;
    }

    // Увеличение технических навыков
    private bool IncreaseTechSkills(Villager villager)
    {
        if (technicalSkills < MaxSkillValue)
        {
            technicalSkills++;
            Debug.Log($"Технические навыки увеличены до {technicalSkills}");
            return true;
        }
        return false;
    }
}
