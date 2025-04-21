using UnityEngine;

public class Background_Touch : MonoBehaviour
{
    public Animator characterAnimator; // ĳ���� �ִϸ�����
    public string triggerName = "Touched"; // Ʈ���� �̸� (�ִϸ����Ϳ� ������ ��)

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���콺 ���� ��ư Ŭ��
        {
            CheckTouch(Input.mousePosition);
            Debug.Log("����");
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) // ��ġ ����
        {
            CheckTouch(Input.GetTouch(0).position);
        }
    }

    void CheckTouch(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("BackGround")) // "BackGround" �±׸� ���� ������Ʈ ��ġ Ȯ��
            {
                characterAnimator.SetTrigger(triggerName);
            }
        }
    }
}
