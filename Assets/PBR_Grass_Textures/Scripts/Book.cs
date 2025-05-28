using UnityEngine;

public class Book : MonoBehaviour
{
    public enum BookType
    {
        Strength,     // Книга силы
        Endurance,    // Книга выносливости
        Technical     // Книга технических навыков
    }

    public BookType bookType; // Тип книги
    public Sprite bookImage;  // Изображение книги (картинка, которая будет отображаться в инвентаре)
    public int buffAmount;    // Сколько увеличивает книга

    // Метод для использования книги
    public void Use(Villager villager)
    {
        switch (bookType)
        {
            case BookType.Strength:
                villager.skills.strength += buffAmount;
                break;
            case BookType.Endurance:
                villager.skills.endurance += buffAmount;
                break;
            case BookType.Technical:
                villager.skills.technicalSkills += buffAmount;
                break;
        }
        villager.UpdateUI();  // Обновляем UI после использования книги
    }
}
