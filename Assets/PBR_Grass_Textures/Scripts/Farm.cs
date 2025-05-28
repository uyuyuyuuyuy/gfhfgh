using UnityEngine;
using System.Collections;

public class Farm : Building
{
    protected override void Start()
    {
        requiredVillagersToBuild = 1; // Ферма доступна при 1 жителе
        base.Start();
        StartCoroutine(ConsumeEnergy());
    }

    protected override void GenerateResources()
    {
        if (villageManager != null && GetWorkerCount() > 0)
        {
            // Получаем коэффициент, зависящий от силы жителя
            float resourceGenerated = resourceAmount * GetResourceCoefficient();
            villageManager.AddResources(resourceGenerated, 0, 0);
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

    // Переопределяем коэффициент для фермы, учитывая силу
    protected override float GetResourceCoefficient()
    {
        if (assignedWorkers.Count > 0)
        {
            float strength = assignedWorkers[0].skills.strength; // Получаем силу первого жителя
            return 0.2f * strength;  // Ресурс генерируется пропорционально силе
        }
        return 1f; // Если нет работников, возвращаем стандартный коэффициент
    }
}
