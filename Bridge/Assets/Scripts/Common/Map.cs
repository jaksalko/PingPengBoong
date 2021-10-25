using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;




[Serializable]
public class Map : IMap
{

    readonly int[,] step = new int[4, 2] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };
    public readonly int mapSizeHeight;
    public readonly int mapSizeWidth;

    public readonly bool parfait;
    public readonly Vector3 characterAPosition;//    y축 -9 : 1 층 , -8 : 2층 
    public readonly Vector3 characterBPosition;

    private List<List<int>> mapBlockDataList;
    public List<List<int>> GetBlockDataList() { return mapBlockDataList; }
    public int GetBlockDataByIndex(int height, int width)
    {
        Debug.Log("Block Data : " + mapBlockDataList[height][width]);
        return mapBlockDataList[height][width];
    }
    public List<List<int>> mapBlockStyleList;
    public List<List<int>> GetBlockStyleList() { return mapBlockStyleList; }
    public List<int> GetBlockSyleByIndex(int index)
    {
        Debug.Log("Block Style Data : " + mapBlockStyleList[index][0]);
        return mapBlockStyleList[index];
    }
    private readonly int[] stepLimits;
    public int[] GetStepLimits() { return stepLimits; }
    public int GetStepLimitsByIndex(int index)
    {
        if (index < stepLimits.Length) return stepLimits[index];
        else return -1;
    }

    public bool[,] check;

    Block[,] blocks;

    public List<Vector2> snowList;
    public List<Block> stepped_blockList;

    public Map(List<List<int>> mapBlockDataList , List<List<int>> mapBlockStyleList, int[] stepLimits, Vector3 characterAPosition, Vector3 characterBPosition, int mapSizeWidth, int mapSizeHeight, bool isParfait)
    {
        this.mapSizeHeight = mapSizeHeight;
        this.mapSizeWidth = mapSizeWidth;

        this.mapBlockDataList = mapBlockDataList;
        this.mapBlockStyleList = mapBlockStyleList;
        this.stepLimits = stepLimits;

        this.characterAPosition = characterAPosition;
        this.characterBPosition = characterBPosition;

        parfait = isParfait;
        check = new bool[mapSizeHeight, mapSizeWidth];
        blocks = new Block[mapSizeHeight, mapSizeWidth];
        snowList = new List<Vector2>();
        stepped_blockList = new List<Block>();
    }


    bool isEndGame()
    {
        for (int i = 0; i < mapSizeHeight; i++)
        {
            for (int j = 0; j < mapSizeWidth; j++)
            {
                if (!check[i, j])
                {
//                    Debug.Log("is false : " + i + "," + j);
                    return false;
                }
            }
        }
        Debug.Log("end game");
        return true;
    }

    List<int> GetThroughBlockList(int floor, int getDirection, bool onCloud)
    {
        switch (floor)
        {
            case 1:
                return BlockNumber.GetDownstairThroughBlock(getDirection, onCloud);
            case 2:
                return BlockNumber.GetUpstairThroughBlock(getDirection, onCloud);
            case 3:
                return BlockNumber.GetThirdFloorThroughBlock(getDirection, onCloud);
                

        }

        return new List<int>();
    }
    List<int> GetStopBlockList(int floor, int getDirection, bool onCloud)
    {
        switch (floor)
        {
            case 1:
                return BlockNumber.GetDownstairStopBlock(getDirection, onCloud);
            case 2:
                return BlockNumber.GetUpstairStopBlock(getDirection, onCloud);
            case 3:
                return BlockNumber.GetThirdFloorStopBlock(getDirection, onCloud);

        }

        return new List<int>();
    }

    //다음 블럭의 데이터와 체크할 리스트(통과, 멈춤, 접근불가 중 하나)를 비교함.
    bool CheckNextBlock(List<int> checkList, int data)
    {
        bool flag = false;

        for (int i = 0; i < checkList.Count; i++)
        {
            if (checkList[i] == data)
            {
                flag = true;
                break;
            }
               
        }
        Debug.Log("Check With " + data + " is " + flag);
        return flag;
    }

    

    bool ChangeState(int next, int nextnext, Player player, ref Vector3Int pos)
    {

        int floor = pos.y;
        int direction = player.getDirection;
        int posX = pos.x;
        int posZ = pos.z;

        switch (floor)
        {
            case 1:
                if (next >= BlockNumber.parfaitA && next <= BlockNumber.parfaitD)
                {
                    //change block data parfait to normal
                    //blocks[posZ + step[direction, 0], posX + step[direction, 1]].data = BlockNumber.normal; //pos 위치가 아닌 한칸 이동한 위치
                    if ((next % 10 - 1) == GameController.ParfaitOrder)
                        GameController.ParfaitOrder++;


                    return true;
                }
                else if(next >= BlockNumber.cloudUp && next <= BlockNumber.cloudLeft)
                {
                    int cloudDirection = (next % 10) - 1;
                    Vector3 targetPosition = new Vector3(posX + step[direction, 1], pos.y, posZ + step[direction, 0]);

                    if (player.getDirection != cloudDirection)
                    {
                        player.getDirection = cloudDirection;
                        //player.targetPositions.Add(new Tuple<Vector3, int>(targetPosition,cloudDirection));
                    }
                    
                    player.onCloud = true;

                    return true;
                }
                else if (next >= BlockNumber.slopeUp && next <= BlockNumber.slopeLeft)
                {
                    int nextFloor = floor + 1;
                    if (CheckNextBlock(GetThroughBlockList(nextFloor, player.getDirection, player.onCloud), nextnext) || CheckNextBlock(GetStopBlockList(nextFloor, player.getDirection, player.onCloud), nextnext))//다음은 지나갈 수 있는 블럭
                    {
                        Debug.Log("floor : 1");
                        //다음 블럭은 올라갈 수 있다
                        pos.y += 1;
                        return true;


                    }
                    else
                    {
                        Debug.Log("cant climb slope...");
                        //올라갈 수 없다 --> 슬로프를 올라가서는 안되므로 false 를 반환.
                        return false;
                    }
                    //player upstair --> true (floor = 1)
                    //if state==master --> other.floor = 2
                    //블럭 리스트 업데이트

                    //슬로프 앞에가 막혀있다면
                    //리턴 false
                    //upstair --> false (floor = 0)
                    //if state==master --> other.floor = 1
                    //다시 블럭리스트 업데이트 
                }
                else
                {
                    return true;
                }

            case 2:
                if (next >= BlockNumber.upperParfaitA && next <= BlockNumber.upperParfaitD)
                {
                    //blocks[posZ + step[direction, 0], posX + step[direction, 1]].data = BlockNumber.upperNormal;
                    if((next%10-1) == GameController.ParfaitOrder)
					    GameController.ParfaitOrder++;
                    Debug.Log("up");
                    return true;
                }
                else if (next >= BlockNumber.cloudUp && next <= BlockNumber.cloudLeft)
                {
                    //높이가 떨어질 경우
                    Vector3 targetPosition = blocks[posZ + step[direction, 0], posX + step[direction, 1]].GetCenterTargetPosition();
                    targetPosition.y = pos.y;
                    player.targetPositions.Add(new Tuple<Vector3, int>(targetPosition, player.getDirection));

                    pos.y -= 1;

                    int cloudDirection = (next % 10) - 1;
                    
                    if (player.getDirection != cloudDirection)
                    {
                        player.getDirection = cloudDirection;
                        //player.targetPositions.Add(new Tuple<Vector3, int>(targetPosition, cloudDirection));
                    }
                    player.onCloud = true;
                    

                    return true;
                }
                else if (next >= BlockNumber.upperCloudUp && next <= BlockNumber.upperCloudLeft)
                {
                    int cloudDirection = (next % 10) - 1;
                    Vector3 targetPosition = new Vector3(posX + step[direction, 1], pos.y, posZ + step[direction, 0]);

                    if (player.getDirection != cloudDirection)
                    {
                        player.getDirection = cloudDirection;
                        //player.targetPositions.Add(new Tuple<Vector3, int>(targetPosition, cloudDirection));
                    }

                    player.onCloud = true;

                    return true;
                }
                else if (next >= BlockNumber.slopeUp && next <= BlockNumber.slopeLeft)
                {
                    int nextFloor = floor - 1;
                    if (CheckNextBlock(GetThroughBlockList(nextFloor, player.getDirection, player.onCloud), nextnext) || CheckNextBlock(GetStopBlockList(nextFloor, player.getDirection, player.onCloud), nextnext))//다음은 지나갈 수 있는 블럭
                    {
                        //다음 블럭은 내려갈 수 있다
                        pos.y -= 1;
                        return true;

                    }
                    else
                    {
                        //내려갈 수 없다 --> 슬로프를 내력가서는 안되므로 false 를 반환.
                        return false;
                    }
                }
                else
                {
                    return true;
                }

            case 3://2층에서는 through 로 들어올 수 없음.?
                if (next >= BlockNumber.cloudUp && next <= BlockNumber.cloudLeft)
                {

                    //높이가 떨어질 경우
                    Vector3 targetPosition = blocks[posZ + step[direction, 0], posX + step[direction, 1]].GetCenterTargetPosition();
                    targetPosition.y = pos.y;
                    player.targetPositions.Add(new Tuple<Vector3, int>(targetPosition, player.getDirection));


                    player.onCloud = true;
                    pos.y -= 2;
                    int cloudDirection = (next % 10) - 1;
                    
                    if (player.getDirection != cloudDirection)
                    {
                        player.getDirection = cloudDirection;
                        //player.targetPositions.Add(new Tuple<Vector3, int>(targetPosition, cloudDirection));
                    }
                    return true;
                }
                else if (next >= BlockNumber.upperCloudUp && next <= BlockNumber.upperCloudLeft)
                {
                   
                    player.onCloud = true;
                    pos.y -= 1;
                    int cloudDirection = (next % 10) - 1;
                    Vector3 targetPosition = new Vector3(posX + step[direction, 1], pos.y, posZ + step[direction, 0]);

                    if (player.getDirection != cloudDirection)
                    {
                        player.getDirection = cloudDirection;
                        //player.targetPositions.Add(new Tuple<Vector3, int>(targetPosition, cloudDirection));
                    }
                    return true;
                }
                return false;

        }

        return false;//error
    }

    
    public void GetDestination(Player player, Vector3Int pos , List<Tuple<Vector3, int>> targetPositions)
    {
        int direction = player.getDirection;

        int floor = pos.y;
        int posX = pos.x;
        int posZ = pos.z;

        if (pos == player.transform.position)//시작부분
        {
            Debug.Log("frist time call by getDestination");
            if (blocks[posZ,posX].type == Block.Type.Cracker)
            {
                Debug.Log("frist time call by getDestination : add cracker block");
                stepped_blockList.Add(blocks[posZ, posX]);
            }
        }

        int next = GetBlockData(x: posX + step[direction, 1], z: posZ+ step[direction, 0]);
        int nextnext = GetBlockData(x: posX + step[direction, 1] * 2, z: posZ + step[direction, 0] * 2);
        Debug.Log(posX+","+posZ + " GetDestination " + (posX + step[direction, 1]) +","+(posZ + step[direction, 0]) + " next data : " + next);



        //다음 블럭이 통과할 수 있는 블럭이라면,
        if (CheckNextBlock(GetThroughBlockList(floor, direction, player.onCloud), next) && ChangeState(next, nextnext, player , ref pos))//다음은 지나갈 수 있는 블럭
        {
            //지나갈 수 있는 블럭
            player.isLock = false;

            posX += step[direction, 1];
            posZ += step[direction, 0];

            //다음블럭 눈을 치웠다고 체크
            UpdateCheckTrue(width: posX, height: posZ);

            //현재 포지션(가상의 펭귄 위치)을 재설정
            pos = new Vector3Int(posX, pos.y, posZ);
            Debug.Log("GetDestination ADD Target Position : " + blocks[posZ, posX].GetCenterTargetPosition());
            targetPositions.Add(new Tuple<Vector3, int>(blocks[posZ, posX].GetCenterTargetPosition(), player.getDirection));

            //게임이 끝나지 않는다면
            if (!isEndGame())
            {
                //다른 캐릭터가 지금 캐릭터에 의해 잠겨있었다면
                Player other = player.other;
                if(other.isLock)
                {
                    //락을 해제하고
                    other.isLock = false;

                    other.GetTargetPositions(this, other.tempBlock.data % 10 - 1);
                    other.Move();
                }

                //바뀐 포지션에서 다시 다음 목적지를 설정
                GetDestination(player, pos , targetPositions);
                return;

            }


        }
        else if (CheckNextBlock(GetStopBlockList(floor, direction, player.onCloud), next))//다음은 멈춰야하는 블럭
        {
            player.onCloud = false; // stop 이면 무조건 oncloud 에서 벗어남.
            player.isLock = false;

            posX += step[direction, 1];
            posZ += step[direction, 0];

            switch (floor)
            {
                case 1://솜사탕 위면 1단계 블럭 또는 열려있는 파르페
                    //솜사탕 위가 아니면 솜사탕에서 멈춤
                    
                    if (next >= BlockNumber.parfaitA && next <= BlockNumber.parfaitD)
                    {
                        player.actionnum = 3;//crash : 3
                        blocks[posZ, posX].data = BlockNumber.normal;
                        if ((next % 10 - 1) == GameController.ParfaitOrder)
                            GameController.ParfaitOrder++;
                    }
                    else
                    {
                        player.actionnum = 3;//crash : 3
                    }
                    break;

                case 2://drop 1-> 0 or ride character
                    if(next == BlockNumber.character)
                    {
                        //ride motion
                        player.actionnum = 2;//ride : 2

                        //player state          Idle --> Slave
                        //other player state    Idle --> Master
                    }
                    else if(next >= BlockNumber.normal && next <= BlockNumber.cracker_2)
                    {
                        //높이가 떨어질 경우
                        Vector3 targetPosition = blocks[posZ, posX].GetCenterTargetPosition();
                        targetPosition.y = pos.y;
                        player.targetPositions.Add(new Tuple<Vector3, int>(targetPosition, player.getDirection));


                        pos.y -= 1;
                        player.actionnum = 5;//drop : 5
                    }
                    else if (next >= BlockNumber.parfaitA && next <= BlockNumber.parfaitD)
                    {
                        //높이가 떨어질 경우
                        Vector3 targetPosition = blocks[posZ, posX].GetCenterTargetPosition();
                        targetPosition.y = pos.y;
                        player.targetPositions.Add(new Tuple<Vector3, int>(targetPosition, player.getDirection));

                        pos.y -= 1;
                        player.actionnum = 5;//drop : 5
                        blocks[posZ, posX].data = BlockNumber.normal;
                        if ((next % 10 - 1) == GameController.ParfaitOrder)
                            GameController.ParfaitOrder++;
                    }
                    else if (next >= BlockNumber.upperParfaitA && next <= BlockNumber.upperParfaitD)//onCloud(2층)에서 2층 파레페 먹고 멈
                    {
                        player.actionnum = 3;// crash : 3
                        blocks[posZ, posX].data = BlockNumber.upperNormal;
                        if ((next % 10 - 1) == GameController.ParfaitOrder)
                            GameController.ParfaitOrder++;
                    }
                    else
                    {
                        player.actionnum = 3;// crash : 3
                    }

                    break;

                case 3://drop 2-> 1 or 0
                    if (next >= BlockNumber.normal && next <= BlockNumber.cracker_2)
                    {
                        //높이가 떨어질 경우
                        Vector3 targetPosition = blocks[posZ, posX].GetCenterTargetPosition();
                        targetPosition.y = pos.y;
                        player.targetPositions.Add(new Tuple<Vector3, int>(targetPosition, player.getDirection));


                        player.actionnum = 5;//drop : 5
                        pos.y -= 2;
                    }
                    else if(next >= BlockNumber.parfaitA && next <= BlockNumber.parfaitD)
                    {
                        //높이가 떨어질 경우
                        Vector3 targetPosition = blocks[posZ, posX].GetCenterTargetPosition();
                        targetPosition.y = pos.y;
                        player.targetPositions.Add(new Tuple<Vector3, int>(targetPosition, player.getDirection));


                        player.actionnum = 5;//drop : 5
                        pos.y -= 2;
                        blocks[posZ, posX].data = BlockNumber.normal;
                        if ((next % 10 - 1) == GameController.ParfaitOrder)
                            GameController.ParfaitOrder++;
                    }
                    else if(next >= BlockNumber.upperNormal && next <= BlockNumber.upperCracker_2)
                    {
                        //높이가 떨어질 경우
                        Vector3 targetPosition = blocks[posZ, posX].GetCenterTargetPosition();
                        targetPosition.y = pos.y;
                        player.targetPositions.Add(new Tuple<Vector3, int>(targetPosition, player.getDirection));


                        player.actionnum = 5;//drop : 5
                        pos.y -= 1;
                    }
                    else if (next >= BlockNumber.upperParfaitA && next <= BlockNumber.upperParfaitD)
                    {
                        //높이가 떨어질 경우
                        Vector3 targetPosition = blocks[posZ, posX].GetCenterTargetPosition();
                        targetPosition.y = pos.y;
                        player.targetPositions.Add(new Tuple<Vector3, int>(targetPosition, player.getDirection));


                        player.actionnum = 5;//drop : 5
                        pos.y -= 1;
                        blocks[posZ, posX].data = BlockNumber.upperNormal;
                        if ((next % 10 - 1) == GameController.ParfaitOrder)
                            GameController.ParfaitOrder++;
                    }
                    else
                    {
                        player.actionnum = 3;//crash : 3
                    }
                    break;
            }//end switch


            
            
            pos = new Vector3Int(posX, pos.y, posZ);
            player.targetPositions.Add(new Tuple<Vector3, int>(blocks[posZ, posX].GetCenterTargetPosition(), player.getDirection));
            Debug.Log("GetDestination ADD Target Position : " + blocks[posZ, posX].GetCenterTargetPosition());
            UpdateCheckTrue(width: posX, height: posZ);

            Player other = player.other;
            if (other.isLock)
            {
                other.isLock = false;
                //other.Move()
                other.GetTargetPositions(this, other.tempBlock.data % 10 - 1);
                other.Move();
            }




        }
        else//cant block
        {
            Debug.Log("cant block : " + pos + " BlockNumber : " + next);

            if((pos.y == 1 && next == BlockNumber.character) || (pos.y == 2 &&next == BlockNumber.upperCharacter))
            {
                player.actionnum = 4; // character끼리 충돌 : 4

                if (player.onCloud)
                    player.isLock = true;
            }
            else if(player.state == Player.State.Master && pos.y == 1 && next >= BlockNumber.upperNormal && next <= BlockNumber.upperGift)
            {
                player.actionnum = 3;
                player.stateChange = true;
            }
            else
            {
                player.actionnum = 3;
            }
        }

        //pos = new Vector3(posX, pos.y, posZ);
        
        //temp
        player.tempBlock = blocks[posZ, posX];
        blocks[posZ, posX] = player;

        //GameController.instance.moveCommand.SetLaterData(snowList, stepped_blockList);

        snowList.Clear();
        stepped_blockList.Clear();

        
    }
    public void UpdateCheckTrue(int width, int height)
    {
        stepped_blockList.Add(blocks[height, width]);
        blocks[height, width].isClear = true;
        check[height, width] = true;
    }

    public void UpdateCheckArray(int width, int height, bool isCheck)
    {
        blocks[height, width].isClear = isCheck;
        check[height, width] = isCheck;
    }

    public int GetBlockData(int x, int z)
    {
        if (x < mapSizeWidth && x >=0 && z < mapSizeHeight && z >= 0)
            return blocks[z, x].data;
        else
            return BlockNumber.obstacle;
    }

    public void SetBlockData(int x, int z , int value)
    {
        blocks[z, x].data = value;
    }

    public void SetBlocks(int width, int height , Block block)
    {
        blocks[height, width] = block;
    }

    public Block GetBlock(int width, int height)
    {
        return blocks[height, width];
    }

	

	
	
}


