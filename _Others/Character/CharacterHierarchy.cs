using BattleCollection;
using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHierarchy : MonoBehaviour
{
    public GameObject shadow;
    [Header("Body")]
    [SerializeField] private SpriteRenderer hairRenderer;
    [SerializeField] private SpriteRenderer faceHairRenderer;
    [SerializeField] private SpriteRenderer headRenderer;
    [SerializeField] private SpriteRenderer[] eyesFrontRenderer;
    [SerializeField] private SpriteRenderer[] eyesBackRenderer;
    [SerializeField] private SpriteRenderer armLRenderer;
    [SerializeField] private SpriteRenderer armRRenderer;
    [Header("Weapon")]
    [SerializeField] private SpriteRenderer weaponRenderer;
    [Header("Clothes")]
    [SerializeField] private SpriteRenderer backRenderer;
    [SerializeField] private SpriteRenderer clothBodyRenderer;
    [SerializeField] private SpriteRenderer clothLeftRenderer;
    [SerializeField] private SpriteRenderer clothRightRenderer;
    [SerializeField] private SpriteRenderer armorBodyRenderer;
    [SerializeField] private SpriteRenderer armorRightRenderer;
    [SerializeField] private SpriteRenderer armorLeftRenderer;
    [SerializeField] private SpriteRenderer helmetRenderer;
    [SerializeField] private SpriteRenderer footRightRenderer;
    [SerializeField] private SpriteRenderer footLeftRenderer;

    public void SetBodySprite(Sprite _hair, Sprite _faceHair, Sprite _eyeFront, Sprite _eyeBack, Sprite _head, Sprite _armL, Sprite _armR, Color _hairColor)
    {
        hairRenderer.sprite = _hair;
        hairRenderer.color = _hairColor;
        faceHairRenderer.sprite = _faceHair;
        faceHairRenderer.color = _hairColor;
        if (_head != null)
            headRenderer.sprite = _head;
        foreach (SpriteRenderer renderer in eyesFrontRenderer)
        {
            if (_eyeFront != null)
                renderer.sprite = _eyeFront;
        }
        foreach (SpriteRenderer renderer in eyesBackRenderer)
        {
            if (_eyeBack != null)
                renderer.sprite = _eyeBack;
        }
        if (_armL != null)
            armLRenderer.sprite = _armL;
        if (_armR != null)
            armRRenderer.sprite = _armR;
    }

    public void SetWeaponSprite(WeaponClass _weapon)
    {
        weaponRenderer.sprite = _weapon.sprite;
    }
    public void SetJob(string _jobId)
    {
        Dictionary<ClothesPart, Sprite> clothesDictPiece = LoadManager.loadManager.clothesDict[_jobId];
        ClothesSet(backRenderer,  ClothesPart.Back);
        ClothesSet(clothBodyRenderer, ClothesPart.ClothBody);
        ClothesSet(clothLeftRenderer,  ClothesPart.ClothLeft);
        ClothesSet(clothRightRenderer,  ClothesPart.ClothRight);
        ClothesSet(armorBodyRenderer,  ClothesPart.ArmorBody);
        ClothesSet(armorRightRenderer,  ClothesPart.ArmorRight);
        ClothesSet(armorLeftRenderer,  ClothesPart.ArmorLeft);
        ClothesSet(helmetRenderer,  ClothesPart.Helmet);
        ClothesSet(footRightRenderer,  ClothesPart.FootRight);
        ClothesSet(footLeftRenderer,  ClothesPart.FootLeft);
        void ClothesSet(SpriteRenderer _spriteRenderer, ClothesPart _clothesPart)
        {
            if (clothesDictPiece.ContainsKey(_clothesPart))
                _spriteRenderer.sprite = clothesDictPiece[_clothesPart];
            else
                _spriteRenderer.sprite = null;
        }
    }




}