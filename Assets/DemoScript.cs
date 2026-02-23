using UnityEngine;

public class DemoScript : MonoBehaviour
{
    
    [SerializeField]
    public int myAddOne;
    public int myAddTwo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        print(addNumbers(myAddOne, myAddTwo));
        print(subtractNumbers(myAddOne, myAddTwo));
        print(multiplyNumbers(myAddOne, myAddTwo));
    }

    public int addNumbers(int myAddOne, int myAddTwo)
    {
        int sum = myAddOne + myAddTwo;
        return sum;
    }
    public int subtractNumbers(int myAddOne, int myAddTwo)
    {
        int difference = myAddOne - myAddTwo;
        return difference;
    }
    public int multiplyNumbers(int myAddOne, int myAddTwo)
    {
        int product = myAddOne * myAddTwo;
        return product;
    }
    
}
