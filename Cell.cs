using UnityEngine;

public class Cell : MonoBehaviour
{

    public Sprite[] sprites; //0 to default, 1 for number 1 .... 9 for bomb; 10 for flag; 11 for question Mark;12 for empty cell; 
    public Sprite realSprite;
    public int bombsAround = 0;
    public bool isVisible = false;
    public bool isBomb = false;
    public bool isFlag = false;
    [SerializeField]private Color selectedColor;
    public SpriteRenderer rend;
    public GameManager gameManager;
    public Vector2Int pos;
    public bool exploded = false;
    int RclickPos = 0;

    void Awake(){
        rend = gameObject.GetComponent<SpriteRenderer>();
        // realSprite = sprites[0];
    }
    public void countBombsAround(){
        for(int y = -1 ; y <=1 ; y++){
            for(int x = -1 ; x <=1 ; x++){
                if(x == 0 && y == 0){
                    continue;
                }
                if(gameManager.isValidPosition(pos.x+x,pos.y+y) &&
                    gameManager.Table[pos.x + x , pos.y+y].GetComponent<Cell>().isBomb){
                    bombsAround++;
                }
            }
        }
    }


    public void updateSprite(){

        if(isBomb){
            realSprite = sprites[9];
        }else if(bombsAround!=0){
            realSprite = sprites[bombsAround];
        }else{
            realSprite = sprites[12];
        }

    }

    public void setVisible(){
        if(isVisible || isFlag){
            return;
        }
        isVisible = true;
        rend.sprite = realSprite;
        if(!isBomb){
            if(bombsAround == 0){
                //recursion
                if(gameManager.isValidPosition(pos.x+1,pos.y)){gameManager.Table[pos.x+1,pos.y].setVisible();}
                if(gameManager.isValidPosition(pos.x,pos.y-1)){gameManager.Table[pos.x,pos.y-1].setVisible();}
                if(gameManager.isValidPosition(pos.x-1,pos.y)){gameManager.Table[pos.x-1,pos.y].setVisible();}
                if(gameManager.isValidPosition(pos.x,pos.y+1)){gameManager.Table[pos.x,pos.y+1].setVisible();}
                if(gameManager.isValidPosition(pos.x+1,pos.y+1)){gameManager.Table[pos.x+1,pos.y+1].setVisible();}
                if(gameManager.isValidPosition(pos.x+1,pos.y-1)){gameManager.Table[pos.x+1,pos.y-1].setVisible();}
                if(gameManager.isValidPosition(pos.x-1,pos.y-1)){gameManager.Table[pos.x-1,pos.y-1].setVisible();}
                if(gameManager.isValidPosition(pos.x-1,pos.y+1)){gameManager.Table[pos.x-1,pos.y+1].setVisible();}
            }
        }
    }


    public void onRClick(){
        if(isVisible){
            return;
        }
        if(isFlag){
            gameManager.flagsAmount--;
        }
        isFlag = false;
        switch(RclickPos){
            case 0:
                //flag;
                rend.sprite = sprites[10];
                RclickPos++;
                gameManager.flagsAmount++;
                isFlag = true;
                break;
            case 1:
                //cuestion mark;
                rend.sprite = sprites[11];
                RclickPos++;
                break;
            default:
                //reset;
                rend.sprite = sprites[0];
                RclickPos = 0;
                break;
        }
    }

    void OnMouseEnter(){
        if(!exploded){
            rend.color = selectedColor;
        }
        gameManager.SelectedCell = this;
    }

    void OnMouseExit(){
        if(exploded){
            return;
        }
        rend.color = Color.white;
        gameManager.SelectedCell = null;

    }




}
