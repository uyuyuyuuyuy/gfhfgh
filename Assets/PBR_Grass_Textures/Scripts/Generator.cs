using UnityEngine;

public class Generator : Building
{
    protected override void Start()
    {
        requiredVillagersToBuild = 3; // Генератор доступен при 3 жителях
        base.Start();
    }

    protected override void GenerateResources()
    {
        if (villageManager != null && GetWorkerCount() > 0)
        {
            // Получаем коэффициент, зависящий от выносливости
            float resourceGenerated = resourceAmount * GetResourceCoefficient();
            villageManager.AddResources(0, 0, resourceGenerated); // Добавляем энергию
        }
    }

    // Переопределяем коэффициент для генератора, учитывая выносливость
    protected override float GetResourceCoefficient()
    {
        if (assignedWorkers.Count > 0)
        {
            float endurance = assignedWorkers[0].skills.endurance; // Получаем выносливость первого работника
            return 0.2f * endurance;  // Ресурс генерируется пропорционально выносливости
        }
        return 1f; // Если нет работников, возвращаем стандартный коэффициент
    }
}
