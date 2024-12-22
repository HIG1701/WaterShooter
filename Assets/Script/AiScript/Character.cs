//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Character : MonoBehaviour
//{
//    [Header("ターン終了判定")] public bool turnFlag;
//    [Header(""), SerializeField] public Vector2Int vector2IntPosition;
//    [Header(""), SerializeField] private CharactersDates charactersDates;
//    [Header(""), SerializeField] private MapGenerator mapGenerator;
//    [Header("マップデータベース")] public List<Vector2Int> rangeTileList = new List<Vector2Int>();
//    public List<int> tileIntNumber = new List<int>();
//    [SerializeField] private Vector2Int[] rangeTile = new Vector2Int[3];
//    [SerializeField] private int[] rangeList = new int[3];
//    private Vector2Int characterPosition;

//    private void Awake()
//    {
//        transform.position = (Vector2)vector2IntPosition;
//        characterPosition = vector2IntPosition;
//        mapGenerator = FindAnyObjectByType<MapGenerator>();
//        mapGenerator.MovementRangeRender(characterPosition, charactersDates, this.gameObject.GetComponent<Character>());

//    }

//    public Vector2Int Vector2IntPosition { get => vector2IntPosition; }


//    public CharactersDates CharaDateRef(CharactersDates charaDates)
//    {
//        charaDates = FindObjectOfType<CharactersDates>();
//        charaDates = charactersDates;
//        return charaDates;
//    }

//    private void CharacterMoveRange()
//    {
        
//    }
//}
