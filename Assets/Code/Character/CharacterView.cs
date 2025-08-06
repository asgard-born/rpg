using UnityEngine;
using UnityEngine.AI;

namespace Character
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
    public class CharacterView : MonoBehaviour
    {
        // Как пример, здесь может быть логика регистрации коллизий в будущем и запуска реактивных команд
        // с передачей game object'ов, с которыми будут работать другие компоненты с нужной зоной ответственности
    }
}