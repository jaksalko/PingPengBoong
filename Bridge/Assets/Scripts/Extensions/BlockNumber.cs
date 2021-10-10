﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
static class BlockNumber
{

    public const int
        //0층 블럭
        normal = 0,
        cloudUp = 1, cloudRight = 2, cloudDown = 3, cloudLeft = 4,
        cracker_0 = 5, cracker_1 = 6, cracker_2 = 7,
        broken = 8,//cracker_3

        //1층 블럭
        upperNormal = 10,
        upperCloudUp = 11, upperCloudRight = 12, upperCloudDown = 13, upperCloudLeft = 14,
        upperCracker_0 = 15, upperCracker_1 = 16, upperCracker_2 = 17,
        upperBroken = 18,//upperCracker_3

        //여기서부터 모양이 기본블럭과 다름
        character = 20,

        parfaitA = 21, parfaitB = 22, parfaitC = 23, parfaitD = 24,
        slopeUp = 25, slopeRight = 26, slopeDown = 27, slopeLeft = 28,
        obstacle = 29,

        //character
        upperCharacter = 30,

        //2층 블럭
        upperParfaitA = 31, upperParfaitB = 32, upperParfaitC = 33, upperParfaitD = 34,
        upperObstacle = 39;


    public static int[] first_floor = new int[] { normal, cloudUp, cloudRight, cloudLeft, cloudDown, cracker_0, cracker_1, cracker_2 };
    public static int[] second_floor = new int[] { upperNormal, upperCloudUp, upperCloudRight, upperCloudLeft, upperCloudDown, upperCracker_0, upperCracker_1, upperCracker_2, slopeUp,slopeRight,slopeDown,slopeLeft,parfaitA,parfaitB,parfaitC,parfaitD,character,obstacle };
    public static int[] third_floor = new int[] { upperCharacter, upperObstacle, upperParfaitA, upperParfaitB, upperParfaitC, upperParfaitD };


    public static int[] firstlevel = new int[4] { normal, cracker_0, cracker_1, cracker_2 };
    public static int[] secondLevel = new int[4] { upperNormal, upperCracker_0, upperCracker_1, upperCracker_2 };
    public static int[] slopeLevel = new int[4] { slopeUp, slopeRight, slopeDown, slopeLeft };

    public static List<int> GetThirdFloorThroughBlock(int dir, bool onCloud)
    {
        List<int> throughBlockList = new List<int>();

        for (int i = 0; i <= 3; i++)
        {
            if (Mathf.Abs(dir - i) != 2)
            {
                throughBlockList.Add(upperCloudUp + i);
                throughBlockList.Add(cloudUp + i);
            }
        }

        return throughBlockList;
    }

    public static List<int> GetUpstairThroughBlock(int dir, bool onCloud)
    {
        int parfaitOrder = GameController.ParfaitOrder;
        List<int> throughBlockList = new List<int>();

        if(!onCloud)
        {
            throughBlockList.AddRange(secondLevel);
            for (int i = 0; i <= parfaitOrder; i++)
            {
                throughBlockList.Add(upperParfaitA + i);
            }
            //throughBlockList.Add(upperParfaitA + parfaitOrder);            
        }

        switch (dir)
        {
            case 0:
                throughBlockList.Add(slopeDown);
                break;
            case 1:
                throughBlockList.Add(slopeLeft);
                break;
            case 2:
                throughBlockList.Add(slopeUp);
                break;
            case 3:
                throughBlockList.Add(slopeRight);
                break;
        }
        

        for (int i = 0; i <= 3; i++)
        {
            if (Mathf.Abs(dir - i) != 2)
            {
                throughBlockList.Add(upperCloudUp + i);
                throughBlockList.Add(cloudUp + i);
            }
        }

        return throughBlockList;
    }
    public static List<int> GetDownstairThroughBlock(int dir, bool onCloud)
    {
        int parfaitOrder = GameController.ParfaitOrder;

        List<int> throughBlockList = new List<int>();

        if(!onCloud)
        {
            throughBlockList.AddRange(firstlevel);
            for(int i = 0; i <= parfaitOrder; i++)
            {
                throughBlockList.Add(parfaitA + i);
            }
            
        }
        
        throughBlockList.Add(slopeUp + dir);
        

        for (int i = 0; i <= 3; i++)
        {
            if (Mathf.Abs(dir - i) != 2)
            {
                throughBlockList.Add(cloudUp + i);
            }
        }

        return throughBlockList;
    }

    #region Stop Block List

    public static List<int> GetThirdFloorStopBlock(int dir, bool onCloud)
    {
        int parfaitOrder = GameController.ParfaitOrder;

        List<int> stopBlockList = new List<int>();
        stopBlockList.AddRange(secondLevel);
        stopBlockList.AddRange(firstlevel);

        for (int i = 0; i <= parfaitOrder; i++)
        {
            stopBlockList.Add(parfaitA + i);//first floor parfait
            stopBlockList.Add(upperParfaitA + i);//first floor parfait
        }


            

        

        return stopBlockList;
    }

    public static List<int> GetUpstairStopBlock(int dir, bool onCloud)
    {
        int parfaitOrder = GameController.ParfaitOrder;

        List<int> stopBlockList = new List<int>();


        if (onCloud)
        {
            stopBlockList.AddRange(secondLevel);
            for (int i = 0; i <= parfaitOrder; i++)
            {
                stopBlockList.Add(upperParfaitA + i);
            }
            //stopBlockList.Add(upperParfaitA + parfaitOrder);
        }

        stopBlockList.AddRange(firstlevel);//normal , cracked
        //stopBlockList.Add(parfaitA + parfaitOrder);//first floor parfait
        for (int i = 0; i <= parfaitOrder; i++)
        {
            stopBlockList.Add(parfaitA + i);
        }


        stopBlockList.Add(character);

        
        return stopBlockList;
    }

    public static List<int> GetDownstairStopBlock(int dir, bool onCloud)
    {
        int parfaitOrder = GameController.ParfaitOrder;

        List<int> stopBlockList = new List<int>();

        if (onCloud)
        {
            stopBlockList.AddRange(firstlevel);
            for (int i = 0; i <= parfaitOrder; i++)
            {
                stopBlockList.Add(parfaitA + i);
            }
            //stopBlockList.Add(parfaitA + parfaitOrder);
        }
        
        return stopBlockList;
    }

    #endregion
}
