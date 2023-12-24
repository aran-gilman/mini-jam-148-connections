using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawn : MonoBehaviour
{
    [SerializeField]
    private List<SpawnEntry> _possibleSpawns;
    [SerializeField]
    private List<int> _pointsPerWave;
    [SerializeField]
    private float _initialDelay;
    [SerializeField]
    private float _waveDelay;
    [SerializeField]
    private float _spawnCooldown;
    [SerializeField]
    private float _numberOfTypesSpawned;
    int _currentWave = 0;

    private void Awake()
    {
        StartCoroutine(WaveCycle());
    }

    IEnumerator WaveCycle()
    {

        //Delay the start of the wave
        if(_currentWave == 0)
        {
            yield return new WaitForSeconds(_initialDelay);
        }
        else
        {
            yield return new WaitForSeconds(_waveDelay);
        }

        //Determine what enemy types will spawn this wave
        int points = _pointsPerWave[_currentWave];
        List<SpawnEntry> chosenEntries = new List<SpawnEntry>();
        for(int i = 0; i < _numberOfTypesSpawned; i++)
        {
            int r = Random.Range(0,_possibleSpawns.Count);
            SpawnEntry candidate = _possibleSpawns[r];
            bool uniqueEnemy = true;
            foreach(SpawnEntry previouslyChosen in chosenEntries)
            {
                //If the candidate enemy is already among the chosen, do not add it again.
                if(candidate.Enemy == previouslyChosen.Enemy)
                {
                    uniqueEnemy = false;
                    break;
                }    
            }
            if (candidate.Threshold <= points && uniqueEnemy)
            {
                chosenEntries.Add(candidate);
            }
        }

        //If no enemy types were chosen, pick the most basic
        if(chosenEntries.Count == 0)
        {
            chosenEntries.Add(_possibleSpawns[0]);
        }

        //Spawn enemies
        for(int i = 0; i < 99; i++)
        {
            int index = i % chosenEntries.Count;
            SpawnEntry currentEntry = chosenEntries[index];
            if (currentEntry.Cost > points)
            {
                break;
            }
            Instantiate(currentEntry.Enemy, transform.position, Quaternion.identity, transform.parent);
            points -= currentEntry.Cost;
            yield return new WaitForSeconds(_spawnCooldown);
        }

        _currentWave++;
        if(_currentWave >= _pointsPerWave.Count)
        {
            _currentWave = _pointsPerWave.Count - 1;
        }

        StartCoroutine(WaveCycle());
    }    

}
