using UnityEngine;
using System.Collections;

public class RopeNode : MonoBehaviour
{
    [SerializeField]
    private Transform otherEnd;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Am I alreayd doing things? If so don't start again but stop doing the balance
        RopeBalance balance = FindObjectOfType<RopeBalance>();

        if (false == balance.isBalancing)
        {
            balance.BeginBalance(transform, otherEnd);
        }
        else
        {
            balance.EndBalance();
        }
    }
    
}
