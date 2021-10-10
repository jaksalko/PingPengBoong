using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class BaseData
{
    public abstract class BaseDataInfo
    {
        public readonly int key;
        public BaseDataInfo(int key)
        {
            this.key = key;
        }
    }

    public abstract void Parsing(List<Dictionary<string, string>> csvDatas);
    public abstract void ClearData();

    
    protected string ParseString(string data)
    {
        string retData = data;
        if (string.IsNullOrEmpty(retData))
        {
            Debug.LogErrorFormat("string is null or empty");
        }
        return retData;
    }

    protected int ParseInt(string data)
    {
        int retData = default;
        if (!int.TryParse(data, out retData))
        {
            Debug.LogErrorFormat("Data dont int exist, data: {0}", data);
        }
        return retData;
    }

    protected float ParseFloat(string data)
    {
        float retData = default;
        if (!float.TryParse(data, out retData))
        {
            Debug.LogErrorFormat("Data dont float exist, data: {0}", data);
        }
        return retData;
    }

    protected long ParseLong(string data)
    {
        long retData = default;
        if (!long.TryParse(data, out retData))
        {
            Debug.LogErrorFormat("Data dont long exist, data: {0}", data);
        }
        return retData;
    }

    protected bool ParseBool(string data)
    {
        bool retData = default;
        if (!bool.TryParse(data, out retData))
        {
            Debug.LogErrorFormat("Data dont bool exist, data: {0}", data);
        }
        return retData;
    }

    protected TEnum ParseEnum<TEnum>(string data) where TEnum : struct
    {
        TEnum retType = default;
        if (!Enum.TryParse<TEnum>(data, out retType))
        {
            Debug.LogErrorFormat("Data dont exist, data: {0}", data);
        }
        return retType;
    }

    protected TEnum[] ParseEnumArr<TEnum>(string data) where TEnum : struct
    {
        TEnum[] retArr = null;
        string dataStr = data;
        if (string.Equals(dataStr, Constants.ExceptionStrKey) == false)
        {
            if (!string.IsNullOrEmpty(dataStr))
            {
                string[] splitStrs = dataStr.Split(';');
                int length = splitStrs.Length;
                retArr = new TEnum[length];
                for (int index = 0; index < length; index++)
                {
                    string tempStr = splitStrs[index];
                    if (!Enum.TryParse<TEnum>(tempStr, out retArr[index]))
                    {
                        Debug.LogErrorFormat("dataStr dont exist, index: {0}, data: {1}", index, data);
                    }
                }
            }
            else
            {
                Debug.LogErrorFormat("data dont intArr exist, data: {0}", data);
            }
        }

        return retArr;
    }

    protected int[] ParseIntArr(string data)
    {
        int[] retArr = null;
        string dataStr = data;
        if (string.Equals(dataStr, Constants.ExceptionStrKey) == false)
        {
            if (!string.IsNullOrEmpty(dataStr))
            {
                string[] splitStrs = dataStr.Split(';');
                int length = splitStrs.Length;
                retArr = new int[length];
                for (int index = 0; index < length; index++)
                {
                    string tempStr = splitStrs[index];
                    if (!int.TryParse(tempStr, out retArr[index]))
                    {
                        Debug.LogErrorFormat("dataStr dont exist, index: {0}, data: {1}", index, data);
                    }
                }
            }
            else
            {
                Debug.LogErrorFormat("data dont intArr exist, data: {0}", data);
            }
        }

        return retArr;
    }

    protected float[] ParseFloatArr(string data)
    {
        float[] retArr = null;
        string dataStr = data;
        if (string.Equals(dataStr, Constants.ExceptionStrKey) == false)
        {
            if (!string.IsNullOrEmpty(dataStr))
            {
                string[] splitStrs = dataStr.Split(';');
                int length = splitStrs.Length;
                retArr = new float[length];
                for (int index = 0; index < length; index++)
                {
                    string tempStr = splitStrs[index];
                    if (!float.TryParse(tempStr, out retArr[index]))
                    {
                        Debug.LogErrorFormat("tempstr dont exist, index: {0}, data: {1}", index, data);
                    }
                }
            }
            else
            {
                Debug.LogErrorFormat("data dont exist, data: {0}", data);
            }
        }

        return retArr;
    }

    protected string[] ParseStringArr(string data)
    {
        string[] retArr = null;
        string dataStr = data;
        if (string.Equals(dataStr, Constants.ExceptionStrKey) == false)
        {
            if (!string.IsNullOrEmpty(dataStr))
            {
                string[] splitStrs = dataStr.Split(';');
                int length = splitStrs.Length;
                retArr = new string[length];
                for (int index = 0; index < length; index++)
                {
                    string tempStr = splitStrs[index];
                    //Debug.Log(tempStr);
                    if (!string.IsNullOrEmpty(tempStr))
                    {
                        retArr[index] = tempStr;
                    }
                    else
                    {
                        Debug.LogErrorFormat("tempstr dont exist, index: {0}, data: {1}", index, data);
                    }
                }
            }
            else
            {
                Debug.LogErrorFormat("data dont exist, data: {0}", data);
            }
        }

        return retArr;
    }

    public Vector3 ParseVector3(string data)
    {
        Vector3 retVec = new Vector3();
        if(!string.IsNullOrEmpty(data))
        {
            string[] splitStrs = data.Split(';');
            int length = splitStrs.Length;
            if(length != 3)
            {
                return default;
            }
            else
            {
                retVec.x = ParseInt(splitStrs[0]);
                retVec.y = ParseInt(splitStrs[1]);
                retVec.z = ParseInt(splitStrs[2]);

                return retVec;
            }
        }
        else
        {
            return default;
        }
    }
}
