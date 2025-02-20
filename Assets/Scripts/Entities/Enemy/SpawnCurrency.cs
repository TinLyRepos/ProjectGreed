using UnityEngine;

public class SpawnCurrency : MonoBehaviour
{
    [SerializeField] private Transform pfPermCurrency = default;
    [SerializeField] private Transform pfTempCurrency = default;
    [SerializeField] private GameObject pfHealthPickup = default;
    [SerializeField] private bool canSpawnHealthPickup = default;
    [SerializeField] private bool canSpawnPermCurrency = default;


    [SerializeField] private int minAmount = 3;
    [SerializeField] private int maxAmount = 8;

    //===========================================================================
    public void SpewOutCurrency()
    {
        int _dropRate = Random.Range(0, 100);

        if (_dropRate < 3)
        {
            if (canSpawnHealthPickup)
                Instantiate(pfHealthPickup, transform.position, Quaternion.identity);
        }
        else if (_dropRate < 10)
        {
            if (canSpawnPermCurrency)
            {
                Vector3 _position = this.transform.position + CultyMarbleHelper.GetRandomDirection() * UnityEngine.Random.Range(0.25f, 0.75f);
                Transform currency = Instantiate(pfPermCurrency, _position, Quaternion.identity);
            }
        }
        else
        {
            int _amount = Random.Range(minAmount, maxAmount);
            for (int i = 0; i < _amount; i++)
            {
                Vector3 _position = this.transform.position + CultyMarbleHelper.GetRandomDirection() * UnityEngine.Random.Range(0.25f, 0.75f);
                Transform currency = Instantiate(pfTempCurrency, _position, Quaternion.identity);
            }
        }
    }
}
