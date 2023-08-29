using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;
public class GameManager : MonoBehaviour
{

    public int total_bombs = 40;
    [SerializeField]private Camera mainCamera;
    private GameObject tableObj;
    public int tableWidth = 80;
    public int tableHeigth = 40;
    public int bombsAmmount = 20;
    public GameObject cellPref;
    public Cell[,] Table;
    public Cell SelectedCell;

    public int flagsAmount = 0;

    public TMP_InputField widthField;
    public TMP_InputField heightField;
    public TMP_InputField bombsField;

    private UiManager uiManager;

    void StartGame(){
        flagsAmount = 0;
        bombsAmmount = total_bombs;
        createTable();
        setBombs();
        calcBombsAround();
        uiManager.hideMainMenu();
        uiManager.UpdateBombsTxt(total_bombs + "");
        uiManager.hideWinnerMenu();


    }
    void Awake()
    {
        uiManager = GetComponent<UiManager>();
    }

    void Update()
    {
        //DEBUG
        if(Input.GetKeyDown(KeyCode.R)){
           StartGame(); 
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            BackToMainMenu();
        }
        //DEBUG

        if(SelectedCell!=null){
            if(Input.GetMouseButtonDown(0)){
                SelectedCell.setVisible();
                if(!SelectedCell.isFlag && SelectedCell.isBomb){
                    SelectedCell.exploded = true;
                    SelectedCell.rend.color = Color.red;
                    gameOver();
                }
            }else if(Input.GetMouseButtonDown(1)){
                SelectedCell.onRClick();
                if(flagsAmount == total_bombs){
                    if(checkWin()){
                        youWin();
                    }
                }
                uiManager.UpdateBombsTxt("" + (total_bombs-flagsAmount));
            }

        }
    }

    void createTable(){
        if(tableObj != null){
            Destroy(tableObj);
        }

        tableObj = new GameObject("Table");        
        Table =  new Cell[tableWidth,tableHeigth];
        for(int y = 0 ; y <tableHeigth ; y++){
             for(int x = 0 ; x < tableWidth ; x++){
                GameObject currCell = Instantiate(cellPref, new Vector2(x,y), Quaternion.identity);
                currCell.name="cell : "+x+"_"+y;
                currCell.transform.parent = tableObj.transform;             
                currCell.GetComponent<Cell>().gameManager = this;
                Table[x,y] = currCell.GetComponent<Cell>();
                Table[x,y].pos = new Vector2Int(x,y);
            }
        }

        tableObj.transform.position = new Vector2(-tableWidth/2, -tableHeigth/2);
    }

    void setBombs(){
        while(bombsAmmount > 0){
            int rX = UnityEngine.Random.Range(0,tableWidth);
            int rY = UnityEngine.Random.Range(0,tableHeigth);
            Cell currentCell = Table[rX,rY];

            if(!currentCell.isBomb){
                currentCell.isBomb = true;
                currentCell.updateSprite();
                bombsAmmount--;
            }

        }
    }
   

    void calcBombsAround(){
        for(int y = 0 ; y<tableHeigth ; y++){
            for(int x = 0 ; x < tableWidth ; x++){
                if(!Table[x,y].isBomb){
                    Table[x,y].countBombsAround();
                    Table[x,y].updateSprite();
                }
            }
        }
    }

    public bool isValidPosition(int x, int y){
        return (x>= 0 && x<tableWidth && y>= 0 && y<tableHeigth);
    }

    public void gameOver(){
        Debug.Log("PERDISTE");
        showAllBombs();
    }


    void showAllBombs(){
        for(int y = 0 ; y <tableHeigth ; y++){
             for(int x = 0 ; x < tableWidth ; x++){
                if(Table[x,y].isBomb){
                    Table[x,y].setVisible();
                }
            }
        }
    }
    void showAllSafeCell(){
        for(int y = 0 ; y <tableHeigth ; y++){
             for(int x = 0 ; x < tableWidth ; x++){
                if(!Table[x,y].isBomb){
                    Table[x,y].setVisible();
                }
            }
        }
    }

    bool checkWin(){
        for(int y = 0 ; y <tableHeigth ; y++){
             for(int x = 0 ; x < tableWidth ; x++){
                if(Table[x,y].isBomb && !Table[x,y].isFlag){
                    return false;
                }
            }
        }
        return true;
    }



    void youWin(){
        // Debug.Log("GANASTE!");
        showAllSafeCell();
        uiManager.showWinnerMenu();
    }



    public void setEasyMode(){
        tableWidth = 8;
        tableHeigth = 8;
        total_bombs = 10;
        StartGame();
    }
    public void setMediumMode(){
        tableWidth = 16;
        tableHeigth = 16;
        total_bombs = 40;
        StartGame();
    }
    public void setHardMode(){
        tableWidth = 31;
        tableHeigth = 16;
        total_bombs = 99;
        StartGame();

    }
    public void setCustomMode(){
        Debug.Log("Custom mode to:");

        String auxw = widthField.text;
        String auxh = heightField.text;
        String auxb =  bombsField.text;
        int w,h,b;
        int.TryParse(auxw, out w);
        int.TryParse(auxh, out h);
        int.TryParse(auxb, out b);
       

        if(w<1 || w > 32 ){
            w = 32;
        }

        if(h<1 || h > 20 ){
            h = 20;
        }

        if(b < 1  || b > w*h){
            b = Mathf.RoundToInt(w*h*3/4);
        }

        tableWidth = w;
        tableHeigth = h;
        total_bombs = b;
        StartGame();
    }


    public void BackToMainMenu(){
        Destroy(tableObj);
        uiManager.showMainMenu();
        uiManager.hideWinnerMenu();
    }

}
