﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TowerManager : SingletonDestroyMono<TowerManager>
{
    private Tower towerSellected;
    public TowerButton towerButtonPressed { get; set; }
    private SpriteRenderer spriteRenderer;  //Setting image to our tower
    private List<Tower> TowerList = new List<Tower>();
    private List<Collider2D> BuildList = new List<Collider2D>();
    private Collider2D buildTile;
    public BuildGround buildSideSellected;
	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildTile = GetComponent<Collider2D>();
        spriteRenderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp(0))
        {
            //worldPoint is the position of the mouse click.
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            /* Ray Cast involves intersecting a ray with the object in an environment.
             * The ray cast tells you what objects in the environment the ray runs into.
             * and may return additional information as well, such as intersection point
             */
            //Finding the worldPoint of where we click, from Vector2.zero (which is buttom left corner)
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            //Check to see if mouse press location is on buildSites

            if (towerSellected != null)
                towerSellected.OnDeSellectedTower();
            if (hit.collider.tag == "buildSite")
            {
                buildSideSellected = hit.collider.gameObject.GetComponent<BuildGround>();
                if (!buildSideSellected.IsBuilded)
                {
                    buildTile = hit.collider;
                    RegisterBuildSite(buildTile);
                    placeTower(hit);   // Build Tower
                }
                else
                {
                    towerSellected = buildSideSellected.GetTowerOnSide();   // Lấy Tower từ build Ground
                    towerSellected.OnSellectTower();    // Chọn Tower
                }
            }
        }

        //When we have a sprite enabled, have it follow the mouse (I.E - Placing a Tower)
        if (spriteRenderer.enabled)
        {
            followMouse();
        }
    }

    public void RegisterBuildSite(Collider2D buildTag)
    {
        BuildList.Add(buildTag);
    }

    public void RegisterTower(Tower tower)
    {
        TowerList.Add(tower);
    }

    public void RenameTagsBuildSites()
    {
        foreach(Collider2D buildTag in BuildList)
        {
            buildTag.tag = "buildSite";
        }
        BuildList.Clear();
    }

    public void DestroyAllTower()
    {
        foreach(Tower tower in TowerList)
        {
            Destroy(tower.gameObject);
        }
        TowerList.Clear();
    }
    //Place new tower on the mouse click location
    public void placeTower(RaycastHit2D hit)
    {
        //If the pointer is not over the Tower Button GameObject && the tower button has been pressed
        //Created new tower at the click location
        if (!EventSystem.current.IsPointerOverGameObject() && towerButtonPressed != null)
        {
            Tower newTower = Instantiate(towerButtonPressed.TowerObject);
            if(buildSideSellected != null)
                buildSideSellected.RegeterBuilderTower(newTower);
            newTower.transform.position = hit.transform.position;
            buyTower(towerButtonPressed.TowerPrice);
            SoundManager.Instance.Play(SoundManager.Instance.TowerBuilt);
            RegisterTower(newTower);
            disableDragSprite();
        }
    }
    public void buyTower(int price)
    {
        GameManager.Instance.SubtractMoney(price);

    }
    public void SelectedTower(TowerButton towerSelected)
    {
        if(towerSelected.TowerPrice <= GameManager.Instance.TotalMoney)
        {
            towerButtonPressed = towerSelected;
            enableDragSprite(towerSelected.DragSprite);
            transform.GetChild(0).localScale = Vector3.one * towerSelected.TowerObject.range * 2;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void followMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    public void enableDragSprite(Sprite sprite)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite; //Set sprite to the one we passed in the parameter
        spriteRenderer.sortingOrder = 10;
    }
    public void disableDragSprite()
    {
        spriteRenderer.enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
