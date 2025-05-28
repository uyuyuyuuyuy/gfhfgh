using UnityEngine;
using System.Collections;

public class WaterStation : Building
{
    protected override void Start()
    {
        requiredVillagersToBuild = 2; // Водная станция доступна при 2 жителях
        base.Start();
        StartCoroutine(ConsumeEnergy());
    }

    protected override void GenerateResources()
    {
        if (villageManager != null && GetWorkerCount() > 0)
        {
            // Получаем коэффициент, зависящий от технических навыков
            float resourceGenerated = resourceAmount * GetResourceCoefficient();
            villageManager.AddResources(0, resourceGenerated, 0); // Добавляем воду
        }
    }

    private IEnumerator ConsumeEnergy()
    {
        while (true)
        {
            yield return new WaitForSeconds(generationInterval);
            if (villageManager != null && GetWorkerCount() > 0)
            {
                villageManager.UseResources(0, 0, 1); // Потребление энергии
            }
        }
    }

    // Переопределяем коэффициент для водной станции, учитывая технические навыки
    protected override float GetResourceCoefficient()
    {
        if (assignedWorkers.Count > 0)
        {
            float technicalSkills = assignedWorkers[0].skills.technicalSkills; // Получаем технические навыки первого работника
            return 0.2f * technicalSkills;  // Ресурс генерируется пропорционально техническим навыкам
        }
        return 1f; // Если нет работников, возвращаем стандартный коэффициент
    }
}
