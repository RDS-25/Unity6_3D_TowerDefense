using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class GameControl : MonoBehaviour
   {
      
      public static int objectCount = 0;
      [SerializeField]
      private Camera mainCamera;
      private Ray ray;
      private RaycastHit raycastHit;
      public bool isClick;
      public List<Transform> tilelist;
      public List<Transform> restlist;
      public List<Transform> restcharlist;
      public List<GameObject> prefabList;
      public Transform selectedTileTransform;
      public GameObject BasicUI;
      public GameObject BulidingTowerUI;
      public GameObject RestTowerUI;
      public Transform InfGroup;
      public bool DefaultState;
      public bool MoveState;

      public Transform ClickedTower;

      private void Awake() 
      {
         //오브젝트 초기화 
         GameObject[] prefabs =Resources.LoadAll<GameObject>("CharPrefabs");
         foreach (GameObject prefab in prefabs)
         {
               prefabList.Add(prefab);
               Debug.Log("Loaded prefab: " + prefab.name);
         }

         mainCamera=Camera.main;
         GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
         foreach (GameObject tile in tiles)
         {
            tilelist.Add(tile.transform);
         }
         GameObject[] rests = GameObject.FindGameObjectsWithTag("Rest");
         foreach (GameObject rest in rests)
         {
            restlist.Add(rest.transform);
         }
         // GameObject의 이름에서 숫자 부분을 기준으로 오름차순으로 restlist를 정렬합니다.
         restlist = restlist.OrderBy(transform => GetNumericValue(transform.gameObject.name)).ToList();

         DefaultState =true;

      }

      int GetNumericValue(string name)
      {
         // 이름 형식이 "rest_#"으로 되어 있다고 가정합니다. 여기서 #는 숫자입니다.
         string numericPart = name.Substring(name.LastIndexOf('_') + 1);
         int number;
         if (int.TryParse(numericPart, out number))
         {
            return number;
         }
         return int.MaxValue; // 파싱에 실패할 경우 큰 숫자를 반환합니다.
      }
      
   
   void Update()
   {
      Lose();
      if(DefaultState){
         if (Input.GetMouseButtonDown(0))
            {
                  // Check if the click is over a UI element
                  if (EventSystem.current.IsPointerOverGameObject())
                  {
                     Debug.Log("Clicked on UI element");
                     return;
                  }
                  // Perform raycast if not clicking on UI element
                  ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                  if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
                  {
                     if (raycastHit.transform.CompareTag("Tile")||raycastHit.transform.CompareTag("Rest"))
                     {
                     isClick = true;
                     selectedTileTransform=raycastHit.transform;
                     ClickedTower = selectedTileTransform.GetComponent<TileSwpan>().Tower;
                     // checkTower();
                     Renderer tileRenderer = raycastHit.transform.GetComponent<Renderer>();
                        if (tileRenderer != null)
                        {
                           //  변경할 색상 설정 (예: 빨간색)
                           DefaultColor();
                           tileRenderer.material.color = Color.red;
                        }
                     }
                  }
                  
         } 
         if(selectedTileTransform != null)
         {
            ShowUi(); 
         }
      }
      if(MoveState){
         moveTile();
      }
      
   }
   public static void AddObject()
   {
      objectCount++;
   }
   public static void RemoveObject()
   {
      objectCount--;
   }

   void Lose(){
      if(objectCount == 3){
         Debug.Log("패배");
      }
   }

      //기본 컬러로
      void DefaultColor()
      {
         for(int i=0; i < tilelist.Count; i++)
         {
            var Renderer =  tilelist[i].GetComponent<Renderer>();
            Renderer.material.color= Color.gray;
         }
          for(int i=0; i < restlist.Count; i++)
         {
            var Renderer =  restlist[i].GetComponent<Renderer>();
            Renderer.material.color= Color.gray;
         }
      }


      void ShowUi()
      {
         if(isClick)
         {
            var TileSwpan = CheckCom(selectedTileTransform);
            if(TileSwpan == null){
               return;
            }
            if(selectedTileTransform.tag =="Tile"){
               if(TileSwpan.IsBuildTower)
               {
                  BulidingTowerUI.SetActive(true);
                  BasicUI.SetActive(false);
                  RestTowerUI.SetActive(false);
                  InfGroup.gameObject.SetActive(true);
                  OpenInfGroups();
               }
               else if(!TileSwpan.IsBuildTower)
               {
                  BasicUI.SetActive(true);
                  InfGroup.gameObject.SetActive(false);
                  BulidingTowerUI.SetActive(false);
                   RestTowerUI.SetActive(false);
               }
            }
            if(selectedTileTransform.tag =="Rest"){
               //원래 rest자리
               if(TileSwpan.IsBuildTower && ClickedTower != null){
                  InfGroup.gameObject.SetActive(true);
                  OpenInfGroups();
                  BulidingTowerUI.SetActive(false);
                  RestTowerUI.SetActive(true);
               }else{
                  closeUI();
               }
            }
         }
      }
      public TileSwpan  CheckCom(Transform selectedTileTransform){
         TileSwpan tileSwpan;
         if (selectedTileTransform.TryGetComponent<TileSwpan>(out tileSwpan)){
            return tileSwpan;
         }else{
            return null;
         }
      }

      void closeUI(){
         BasicUI.SetActive(false);
         BulidingTowerUI.SetActive(false);
         RestTowerUI.SetActive(false);
         InfGroup.gameObject.SetActive(false);
         DefaultColor(); 
      }

      //버튼 누를때 실행 타워 생성
      public void SpawnTower(){
         var a = selectedTileTransform.GetComponent<TileSwpan>();
         if(a.IsBuildTower ==true){
            Debug.Log("이미 타워가 건설되어 있습니다.");
            return;
         }
         if (selectedTileTransform != null)
         {
            a.IsBuildTower= true;
               Vector3 newPosition = new Vector3(selectedTileTransform.position.x, selectedTileTransform.position.y + 1f, selectedTileTransform.position.z);
               GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
               foreach(GameObject prefabList in prefabList){
                  if(prefabList.name == clickedButton.name){
                     Instantiate(prefabList, newPosition, Quaternion.identity);
                     isClick=false;
                     selectedTileTransform=null;
                     closeUI();
                  }
               }
         }
         else
         {
               Debug.Log("No tile selected for tower spawn.");
         }
      }

      public void TakeRest(){
         var TileSwpan = selectedTileTransform.GetComponent<TileSwpan>();
         if(ClickedTower==null){
            Debug.Log("선택된 타워가 없습니다.");
            return;
         }
         if(restlist.Count > restcharlist.Count){
            AddValueAtFirstEmptyOrNull(ClickedTower);
            int index = restcharlist.IndexOf(ClickedTower);
            Vector3 newPos = new Vector3(restlist[index].position.x,restlist[index].position.y+0.7f,restlist[index].position.z);
            ClickedTower.position = newPos;
            ClickedTower.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            var restcube = restlist[index].GetComponent<TileSwpan>();
            restcube.IsBuildTower = true;
            TileSwpan.IsBuildTower= false;
            ClickedTower.GetComponent<Char>().isRest = true;
            isClick=false;
            closeUI();
         }else{
            Debug.Log("자리가 다찼음");
         }
      }

      public void MoveStateture(){
         MoveState = true;
         DefaultState =false;
      }
      public void DefaultStatetrue(){
         MoveState = false;
         DefaultState =true;
      }

      void moveTile(){
         if(MoveState){
            if (Input.GetMouseButtonDown(0)){
                  ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                     if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
                     {
                        if (raycastHit.transform.CompareTag("Tile"))
                        {
                           selectedTileTransform=raycastHit.transform;
                           var TileSwpan = selectedTileTransform.GetComponent<TileSwpan>();
                           
                           // var a = checkTower();
                           if(TileSwpan.IsBuildTower)
                           {
                              Debug.Log("이미 소환된 타워가 있어서 이동이 안되요. ");
                           }
                           else if(!TileSwpan.IsBuildTower)
                           {
                              Vector3 newPos = new Vector3(raycastHit.transform.position.x,raycastHit.transform.position.y+1.0f,raycastHit.transform.position.z);
                              ClickedTower.transform.position = newPos;
                              int index = restcharlist.IndexOf(ClickedTower);
                              RemoveAt(index);
                              DefaultStatetrue();
                              closeUI();
                              isClick= false;
                              ClickedTower.GetComponent<Char>().isRest =false;
                              
                           }
                        }
                     }
            }
         }
      }
  //대기 공간 위치 찾아서 집어넣기
   void AddValueAtFirstEmptyOrNull(Transform newValue)
   {
      for (int i = 0; i < restcharlist.Count; i++)
      {
         if (restcharlist[i] == null)
         {
               restcharlist[i] = newValue;
               return; // 첫 번째 null을 찾고 값을 넣은 후 메서드를 종료
         }
      }
       // 만약 null 또는 빈 공간이 없다면, 리스트의 끝에 새 값을 추가
      restcharlist.Add(newValue);
   }

   void RemoveAt(int index)
   {
     if (index >= 0 && index < restcharlist.Count)
     {
           // 특정 인덱스의 값을 null로 설정
         restcharlist[index] = null;
      }
     else
     {
      Debug.LogError("Index out of range.");
     }
   }

   // void RemoveValue(Transform value)
   // {
   //    int index = restcharlist.IndexOf(value);
   //    if (index >= 0)
   //    {
   //       restcharlist[index] = null;
   //    }
   // }
   
   void OpenInfGroups(){//캐릭터 정보
      var TxtDamage = InfGroup.GetChild(2).GetComponent<TextMeshProUGUI>();
      var TxtAttackSpeed =  InfGroup.GetChild(3).GetComponent<TextMeshProUGUI>();
      
      if(TxtDamage != null){
         TxtDamage.text=TxtDamage.name+":"+ ClickedTower.GetComponent<Char>().attackDamage.ToString();
      }else{
         Debug.Log(TxtDamage);
      }
       if(TxtAttackSpeed != null){
         TxtAttackSpeed.text =TxtAttackSpeed.name+":"+ CalculateAttackSpeed(ClickedTower.GetComponent<Char>().attackInterval).ToString();
      }else{
         Debug.Log(TxtAttackSpeed);
      }
   }

      // 공격 속도를 계산하는 메서드입니다.
      public float CalculateAttackSpeed(float interval)
      {
         if (interval <= 0)
         {
            Debug.LogError("Shoot interval must be greater than zero.");
            return 0;
         }
         float speed = 1 / interval;
         return  Mathf.Round(speed *100f)/100f;
      }
}





