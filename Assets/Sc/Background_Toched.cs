using UnityEngine;

public class Background_Touch : MonoBehaviour
{
    public Animator characterAnimator; // 캐릭터 애니메이터
    public string triggerName = "Touched"; // 트리거 이름 (애니메이터에 설정된 값)

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭
        {
            CheckTouch(Input.mousePosition);
            Debug.Log("왼쪽");
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) // 터치 시작
        {
            CheckTouch(Input.GetTouch(0).position);
        }
    }

    void CheckTouch(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("BackGround")) // "BackGround" 태그를 가진 오브젝트 터치 확인
            {
                characterAnimator.SetTrigger(triggerName);
            }
        }
    }
}
