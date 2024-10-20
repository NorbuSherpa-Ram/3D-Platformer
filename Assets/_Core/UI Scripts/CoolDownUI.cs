using UnityEngine;
using UnityEngine.UI;

public class CoolDownUI : MonoBehaviour
{
    public Image coolDownImage;

    public void SetCoolDown()
    {
        if (coolDownImage.fillAmount <= 0)
        {
            coolDownImage.fillAmount = 1;
        }
    }

    public void CheckCoolDown(float _coolDown)
    {
        if (coolDownImage.fillAmount > 0)
        {
            coolDownImage.fillAmount -= 1 / _coolDown * Time.deltaTime;
        }
    }

}
