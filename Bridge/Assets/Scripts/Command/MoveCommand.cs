using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
public class MoveCommand : ICommand
{

	public struct PlayerState
    {
		public Player.State state;
		public bool onCloud;
		public bool isLock;
		public Block temp;

		public PlayerState(Player.State state_ , bool cloud , bool isLock_ , Block tmp)
        {
			state = state_;
			onCloud = cloud;
			isLock = isLock_;
			temp = tmp;
        }
    }

	//make move command
	Player player;//움직일 player
	int dir;//방향
	Map map;//플레이어가 움직일 맵

	//before data
	Vector3 beforePosition_move;
	Vector3 beforePosition_other;

	PlayerState player_move_state;
	PlayerState player_other_state;


	Block[,] beforeBlocksData;
	Block[,] beforeBlocks;
	bool[,] beforeChecks;

	readonly int beforeParfaitOrder;
	readonly int moveCount;



	public MoveCommand(Player p, Map m, int direction)
	{
		player = p;
		map = m;
		dir = direction;

		Player player_move = player;
		Player player_other = player.other;

		player_move_state = new PlayerState(player_move.state, player_move.onCloud, player_move.isLock, player_move.tempBlock);
		player_other_state = new PlayerState(player_other.state, player_other.onCloud, player_other.isLock, player_other.tempBlock);

		beforePosition_move = player_move.transform.position;
		beforePosition_other = player_other.transform.position;

		beforeParfaitOrder = GameController.ParfaitOrder;
		moveCount = GameController.instance.moveCount;

		int mapHeight = map.mapSizeHeight;
		int mapWidth = map.mapSizeWidth;
		beforeBlocks = new Block[mapHeight, mapWidth];
		beforeBlocksData = new Block[mapHeight, mapWidth];
		beforeChecks = new bool[mapHeight, mapWidth];


		for(int height = 0; height < mapHeight; height++)
        {
			for(int width = 0; width < mapWidth; width++)
            {
				beforeBlocks[height, width] = map.GetBlock(width, height);

				Block block =  map.GetBlock(width, height).ShallowCopy();
				beforeBlocksData[height, width] = block;
				if(map.check[height, width])
					Debug.Log(height + "," + width + " is true");
				beforeChecks[height, width] = map.check[height, width];
            }
        }

		
	}

	
	public void Execute()
	{
		player.Move();
	}

	public void Undo()
	{
		/*RETURN TO BEFORE STATE
         * GameController
         * Player(Both)
        */

		GameController.ParfaitOrder = beforeParfaitOrder;
		GameController.instance.moveCount = moveCount;
		
		int mapHeight = map.mapSizeHeight;
		int mapWidth = map.mapSizeWidth;

		for (int height = 1; height < mapHeight-1; height++)
		{
			for (int width = 1; width < mapWidth-1; width++)
			{
				map.check[height, width] = beforeChecks[height, width];
				map.SetBlocks(width, height, beforeBlocks[height, width]);
				map.GetBlock(width, height).RevertBlock(beforeBlocksData[height, width]);
			}
		}



		Player main_player = player;//먼저 움직인 캐릭터
		Player other_player = player.other;//그 다음에 움직인 캐릭터(안 움직였을 수도 있음)

		main_player.transform.position = beforePosition_move;
		other_player.transform.position = beforePosition_other;

		Debug.Log(player_move_state.state);

		main_player.tempBlock = player_move_state.temp;
		if(main_player.tempBlock.type == Block.Type.Cracker)
        {
			CrackedBlock crackedBlock = (CrackedBlock)main_player.tempBlock;
			crackedBlock.data--;
			crackedBlock.count--;
			crackedBlock.SetMaterial();
        }
		main_player.onCloud = player_move_state.onCloud;
		main_player.isLock = player_move_state.isLock;
		main_player.state = player_move_state.state;

		other_player.tempBlock = player_other_state.temp;
		if (other_player.tempBlock.type == Block.Type.Cracker)
		{
			CrackedBlock crackedBlock = (CrackedBlock)other_player.tempBlock;
			crackedBlock.data--;
			crackedBlock.count--;
			crackedBlock.SetMaterial();
		}
		other_player.onCloud = player_other_state.onCloud;
		other_player.isLock = player_other_state.isLock;
		other_player.state = player_other_state.state;

		if(main_player.state == Player.State.Master)
        {
			other_player.transform.SetParent(main_player.transform);
        }
		else if(main_player.state == Player.State.Slave)
        {
			main_player.transform.SetParent(other_player.transform);
		}
		else
        {
			other_player.transform.SetParent(null);
			main_player.transform.SetParent(null);
		}


		GameController.instance.UndoCommand();
		GameController.instance.RemainCheck();


	}





}