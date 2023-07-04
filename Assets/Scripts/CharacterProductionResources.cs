using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterProductionResources", menuName = "Productions", order = int.MinValue)]
public class CharacterProductionResources : ScriptableObject
{
    public List<Sprite> effectBackGround = new List<Sprite>();
    public List<Sprite> effectFront = new List<Sprite>();
    public Sprite illust;
    public Sprite illustBack;
    public Sprite characterName;
}
