using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test
{
    string tt = "";
    public string str
    {
        get {
            return tt;
        }
        set {
            tt = value;
            back();
        }
    }

    void back()
    {
        Debug.Log("======");
    }
}


public class Test<T>
{
    private T _value;
    public Action<T> OnValueChange;
    public T Value {
        get
        {
            return _value;
        }
        set {
            _value = value;
            OnValueChange?.Invoke(_value);
        }
    }
}


public class TEst : MonoBehaviour
{
    void Start()
    {
        Test<string> test = new Test<string>();
        test.OnValueChange = delegate(string str){
            Debug.Log("change value is : " + str);
        };

        test.Value = "hello";
        test.Value = "World";
    }

    void Update()
    {
        
    }
}
