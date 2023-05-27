using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { Spawning, Waiting, Counting };

    public Wave[] waves;
    public Transform[] zombieSpawnPoints;

    public GameObject[] healthSpawnPoints;
    public GameObject[] ammoSpawnPoints;
    public GameObject aKSpawnPoints;
    public GameObject pistolSpawnPoints;
    public GameObject nextLevel;

    public GameObject[] lights;
    public GameObject[] enemyLights;
    FlashingLight flashingLight;

    public float timeBetweenWaves = 3f;
    public float waveCountDown;

    private float searchCountdown = 1f;
    public int nextWave = 0;
    private SpawnState state = SpawnState.Counting;

    // Start is called before the first frame update
    void Start()
    {
        if(zombieSpawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points");
        }

        foreach (GameObject obj in enemyLights)
        {
            obj.SetActive(false);
        }

        flashingLight = GetComponent<FlashingLight>();
        waveCountDown = timeBetweenWaves;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == SpawnState.Waiting)
        {
            if (!ZombieIsAlive())
            {
                nextLevel.SetActive(true);
                StartCoroutine(UI());
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if(waveCountDown <= 0)
        {
            if(state != SpawnState.Spawning)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }
    }

    IEnumerator UI()
    {
        yield return new WaitForSeconds(2);

        nextLevel.SetActive(false);
    }

    void WaveCompleted()
    {
        //GameObject[] deadZombies = GameObject.FindGameObjectsWithTag("RagdollDead");
        state = SpawnState.Counting;
        waveCountDown = timeBetweenWaves;

        SetActiveTrue();

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;

            /*
            foreach (GameObject name in deadZombies)
            {
                Destroy(name.gameObject);
            }
            */
        }
        else
        {
            nextWave++;
        }

    }

    private void SetActiveTrue()
    {
        foreach (GameObject obj in healthSpawnPoints)
        {
            obj.SetActive(true);
        }

        foreach (GameObject obj in ammoSpawnPoints)
        {
            obj.SetActive(true);
        }

        foreach (GameObject obj in lights)
        {
            obj.SetActive(true);
        }

        foreach (GameObject obj in enemyLights)
        {
            obj.SetActive(false);
        }

        //aKSpawnPoints.SetActive(true);

        //pistolSpawnPoints.SetActive(true);
    }

    bool ZombieIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if(searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Ragdoll") == null)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave wave)
    {
        state = SpawnState.Spawning;

        foreach (GameObject obj in lights)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in enemyLights)
        {
            obj.SetActive(true);
        }

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.zombie);
            yield return new WaitForSeconds(1f / wave.rate);
        }

        state = SpawnState.Waiting;
        yield break;
    }

    void SpawnEnemy(Transform zombie)
    {
        Transform sp = zombieSpawnPoints[Random.Range (0, zombieSpawnPoints.Length)];
        Instantiate(zombie, sp.position, sp.rotation);
    }
}

[System.Serializable]
public class Wave
{
    public string name;
    public Transform zombie;
    public int count;
    public float rate;
}