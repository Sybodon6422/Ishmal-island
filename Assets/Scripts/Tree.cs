using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour,IDamagable
{
    public float health = 100;
    public int treeTier;

    public Type treeType;

    [SerializeField] GameObject treeLogOBJ, treeFelledOBJ, treeHalfOBJ, treeStumpOBJ;

    [SerializeField] float treeWidth, treeHeight;

    public enum Type
    {
        Tree,
        FelledTree,
        LogHalf,
        Log,
        Stump
    }

    [SerializeField] Transform leaves;

    public void Create(float height, float width)
    {
        treeWidth = width;
        treeHeight = height;
        if (treeType == Type.Tree)
        {
            health = 250 * width;
            transform.localScale = new Vector3(width, height, width);
            leaves.localPosition = new Vector3(0, 5);
            leaves.localScale = new Vector3(1+ (width*1), 1 + (width * 1), (height / 5));
        }
        health = Mathf.FloorToInt(100 * width);
    }

    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    void Dead()
    {
        switch (treeType)
        {
            case Type.Tree:
                Destroy(leaves.gameObject);
                Destroy(transform.gameObject);

                var stump = Instantiate(treeStumpOBJ, transform.position, transform.rotation);
                stump.GetComponent<Tree>().Create(treeHeight, treeWidth);
                stump.transform.localScale = new Vector3(treeWidth, .2f, treeWidth);

                Vector3 offset = new Vector3(0, .22f, 0);
                var go = Instantiate(treeFelledOBJ, transform.position + offset, transform.rotation * Quaternion.Euler(Random.Range(-1.5f, +1.5f), 0, Random.Range(-1.5f, +1.5f)));

                go.GetComponent<Tree>().Create(treeHeight -.22f, treeWidth);
                go.GetComponent<Rigidbody>().AddForce(Random.Range(-.1f, .1f), 0, Random.Range(-.1f, .1f));
                go.transform.localScale = new Vector3(treeWidth, treeHeight - .22f, treeWidth);

                break;
            case Type.FelledTree:
                Destroy(transform.gameObject);

                Vector3 offsetF = new Vector3(0, .22f, 0);
                CreateLogHalf(
                    transform.position + (transform.up * .22f), transform.rotation, treeHeight / 2);
                offsetF.y += treeHeight / 2;
                CreateLogHalf(
                    transform.position + (transform.up * treeHeight), transform.rotation, treeHeight / 2);

                break;
            case Type.Log:
                break;
            case Type.LogHalf:
                Destroy(transform.gameObject);
                int numOfLogs = Mathf.FloorToInt(treeHeight / 2);
                for (int i = 0; i < numOfLogs; i++)
                {
                    CreateLog(transform.position +
                        (transform.up*(i*2))
                        , transform.rotation,2);
                }
                break;
            case Type.Stump:
                break;
            default:
                break;
        }
    }

    void CreateLogHalf(Vector3 _Logposition, Quaternion _LogRotation, float _LogHeight)
    {
        var go = Instantiate(treeHalfOBJ, _Logposition, _LogRotation * Quaternion.Euler(Random.Range(-1.5f, +1.5f), 0, Random.Range(-1.5f, +1.5f)));
        go.GetComponent<Tree>().Create(treeHeight/2, treeWidth);
        go.GetComponent<Rigidbody>().AddForce(Random.Range(-.1f, .1f), 0, Random.Range(-.1f, .1f));
        go.transform.localScale = new Vector3(treeWidth, _LogHeight, treeWidth);
    }
    void CreateLog(Vector3 _Logposition,Quaternion _LogRotation, float _LogHeight)
    {
        var go = Instantiate(treeLogOBJ, _Logposition, _LogRotation * Quaternion.Euler(Random.Range(-1.5f, +1.5f), 0, Random.Range(-1.5f, +1.5f)));
        go.GetComponent<WorldItem>().itemWeight = treeWidth * 100;
        go.transform.localScale = new Vector3(treeWidth, _LogHeight, treeWidth);
    }
    AudioSource sound;
    public void Damage(float damage, WorldItem _Item, int tier, PlayerSpecifications specs)
    {
        if(specs == null) { return; }
        if(_Item == null) { return; }
        if (_Item.linkedItem.HasSpecialProperty(ItemSpecialProperty.Chopping))
        {
            if (tier >= treeTier)
            {
                sound.Play();
                health -= damage;
                if (health <= 0)
                {
                    Dead();
                    specs.AddXP(5);
                    return;
                }
            }
        }
    }

}
