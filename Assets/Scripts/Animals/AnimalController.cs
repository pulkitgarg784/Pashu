using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class AnimalController : MonoBehaviour
{
    private Stats stats;
    public Material[] skins;
    public Renderer meshRenderer;
    public enum State
    {
        Wander,
        goToFood,
        goToWater,
        Dead
    }
    public State currentState;

    public NavMeshAgent agent;
    Vector3 target;
    public Animator animator;
    public bool rotateOnDeath;
    public float runSpeed;
    public float walkSpeed;

    //Wander
    public float wanderRadius;
    public float wanderTimer;
    private float timer;

    //Stats
    [Header("Stats:")]
    public string animalType;
    public float health = 100;
    public float water = 100;
    public float healthReductionSpeed = 2;

    [Header("Indicators:")]
    public GameObject foodIndicator;
    public GameObject waterIndicator;
    public Text deathText;
    bool isDead;

    //Name and age
    private string[] catNames = new string[] { "Oliver", "Leo", "Milo", "Charlie", "Max", "Jack", "Simba", "Loki", "Oscar", "Jasper", "Buddy", "Tiger", "Toby", "George", "Smokey", "Simon", "Tigger", "Ollie", "Louie", "Felix", "Dexter", "Shadow", "Finn", "Henry", "Kitty", "Oreo", "Gus", "Binx", "Winston", "Sam", "Rocky", "Gizmo", "Sammy", "Jax", "Sebastian", "Blu", "Theo", "Beau", "Salem", "Chester", "Lucky", "Frankie", "Boots", "Cooper", "Thor", "Bear", "Romeo", "Teddy", "Bandit", "Ziggy", "Apollo", "Pumpkin", "Boo", "Zeus", "Bob", "Tucker", "Jackson", "Tom", "Cosmo", "Bruce", "Murphy", "Buster", "Midnight", "Moose", "Merlin", "Frank", "Joey", "Thomas", "Harley", "Prince", "Archie", "Tommy", "Marley", "Otis", "Casper", "Harry", "Benny", "Percy", "Bentley", "Jake", "Ozzy", "Ash", "Sylvester", "Mickey", "Fred", "Walter", "Clyde", "Pepper", "Calvin", "Tux", "Stanley", "Garfield", "Louis", "Mowgli", "Mac", "Luke", "Sunny", "Duke", "Hobbes", "Remi", "Luna", "Bella", "Lily", "Lucy", "Kitty", "Callie", "Nala", "Zoe", "Chloe", "Sophie", "Daisy", "Stella", "Cleo", "Lola", "Gracie", "Mia", "Molly", "Penny", "Willow", "Olive", "Kiki", "Pepper", "Princess", "Rosie", "Ellie", "Maggie", "Coco", "Piper", "Lulu", "Sadie", "Izzy", "Ginger", "Abby", "Sasha", "Pumpkin", "Ruby", "Shadow", "Phoebe", "Millie", "Roxy", "Minnie", "Baby", "Fiona", "Jasmine", "Penelope", "Sassy", "Charlie", "Oreo", "Mittens", "Boo", "Belle", "Misty", "Mimi", "Missy", "Emma", "Annie", "Athena", "Hazel", "Angel", "Ella", "Cookie", "Bailey", "Arya", "Nova", "Olivia", "Zelda", "Maya", "Smokey", "Peanut", "Poppy", "Midnight", "Winnie", "Patches", "Charlotte", "Layla", "Leia", "Delilah", "Alice", "Harley", "Pearl", "Ivy", "Lexi", "Peaches", "Mila", "Romani", "Miss Kitty", "Kitten", "Cat", "Snickers", "Scout", "Blu", "Lucky", "Freya", "Tiger", "Stormy", "Jade", "Honey", "Marley", "Frankie", "Gigi" };
    private string[] dogNames = new string[] { "Abe", "Abbott", "Ace", "Aero", "Aiden", "AJ", "Albert", "Alden", "Alex", "Alfie", "Alvin", "Amos", "Andy", "Angus", "Apollo", "Archie", "Aries", "Artie", "Ash", "Austin", "Axel", "Bailey", "Bandit", "Barkley", "Barney", "Baron", "Baxter", "Bear", "Beau", "Benji", "Benny", "Bentley", "Billy", "Bingo", "Blake", "Blaze", "Blue", "Bo", "Boomer", "Brady", "Brody", "Brownie", "Bruce", "Bruno", "Brutus", "Bubba", "Buck", "Buddy", "Buster", "Butch", "Buzz", "Cain", "Captain", "Carter", "Cash", "Casper", "Champ", "Chance", "Charlie", "Chase", "Chester", "Chewy", "Chico", "Chief", "Chip", "CJ", "Clifford", "Clyde", "Coco", "Cody", "Colby", "Cooper", "Copper", "Damien", "Dane", "Dante", "Denver", "Dexter", "Diego", "Diesel", "Dodge", "Drew", "Duke", "Dylan", "Eddie", "Eli", "Elmer", "Emmett", "Evan", "Felix", "Finn", "Fisher", "Flash", "Frankie", "Freddy", "Fritz", "Gage", "George", "Gizmo", "Goose", "Gordie", "Griffin", "Gunner", "Hank", "Harley", "Harvey", "Hawkeye", "Henry", "Hoss", "Huck", "Hunter", "Iggy", "Ivan", "Jack", "Jackson", "Jake", "Jasper", "Jax", "Jesse", "Joey", "Johnny", "Judge", "Kane", "King", "Kobe", "Koda", "Lenny", "Leo", "Leroy", "Levi", "Lewis", "Logan", "Loki", "Louie", "Lucky", "Luke", "Marley", "Marty", "Maverick", "Max", "Maximus", "Mickey", "Miles", "Milo", "Moe", "Moose", "Morris", "Murphy", "Ned", "Nelson", "Nero", "Nico", "Noah", "Norm", "Oakley", "Odie", "Odin", "Oliver", "Ollie", "Oreo", "Oscar", "Otis", "Otto", "Ozzy", "Pablo", "Parker", "Peanut", "Pepper", "Petey", "Porter", "Prince", "Quincy", "Radar", "Ralph", "Rambo", "Ranger", "Rascal", "Rebel", "Reese", "Reggie", "Remy", "Rex", "Ricky", "Rider", "Riley", "Ringo", "Rocco", "Rockwell", "Rocky", "Romeo", "Rosco", "Rudy", "Rufus", "Rusty", "Sam", "Sammy", "Samson", "Sarge", "Sawyer", "Scooby", "Scooter", "Scout", "Abby", "Addie", "Alexis", "Alice", "Allie", "Alyssa", "Amber", "Angel", "Anna", "Annie", "Ariel", "Ashley", "Aspen", "Athena", "Autumn", "Ava ", "Avery", "Baby", "Bailey", "Basil", "Bean", "Bella", "Belle", "Betsy", "Betty", "Bianca", "Birdie", "Biscuit", "Blondie", "Blossom", "Bonnie", "Brandy", "Brooklyn", "Brownie", "Buffy", "Callie", "Camilla", "Candy", "Carla", "Carly", "Carmela", "Casey", "Cassie", "Chance", "Chanel", "Chloe", "Cutie ", "Cleo", "Coco", "Cookie", "Cricket", "Daisy", "Dakota", "Dana", "Daphne", "Darla", "Darlene", "Delia", "Delilah", "Destiny", "Diamond", "Diva", "Dixie", "Dolly", "Duchess", "Eden", "Edie", "Ella", "Ellie", "Elsa", "Emma", "Emmy", "Eva ", "Faith", "Fanny", "Fern", "Fiona", "Foxy", "Gabby", "Gemma", "Georgia", "Gia ", "Gidget", "Gigi", "Ginger", "Goldie", "Grace", "Gracie", "Greta", "Gypsy", "Hailey", "Hannah", "Harley", "Harper", "Hazel", "Heidi", "Hershey", "Holly", "Honey", "Hope", "Ibby", "Inez", "Isabella", "IvyGus", "Izzy", "Jackie", "Jada", "Jade", "Jasmine", "Jenna", "Jersey", "Jessie", "Jill", "Josie", "Julia", "Juliet", "Juno", "Kali", "Kallie", "Karma", "Kate", "Katie", "Kayla", "Kelsey", "Khloe", "Kiki", "Kira", "Koko", "Kona", "Lacy", "Lady", "Layla", "Leia", "Lena", "Lexi", "Libby", "Liberty", "Lily", "Lizzy", "Lola", "London", "Lucky", "Lulu", "Luna", "Mabel", "Mackenzi", "Macy", "Maddie", "Madison", "Maggie", "Maisy", "Mandy", "Marley", "Matilda", "Mattie", "Maya", "Mia ", "Mika", "Mila", "Miley", "Millie", "Mimi", "Minnie", "Missy", "Misty", "Mitzi", "Mocha", "Molly", "Morgan", "Moxie", "Muffin", "Mya ", "Nala", "Nell", "Nellie", "Nikki", "Nina", "Noel", "Nola", "Nori", "Olive", "Olivia", "Oreo", "Paisley", "Pandora", "Paris", "Peaches", "Peanut", "Pearl", "Pebbles", "Penny", "Pepper", "Phoebe", "Piper", "Pippa", "Pixie", "Polly", "Poppy", " ", "Raven", "Reese" };
    private string[] rabbitNames = new string[] { "Hopscotch", "Carrots", "Bubbles", "Cheerio", "Gepetto", "Kitten", "Mittens", "Cupcake", "Pickles", "Popsicle", "Polka Dot", "Buttons", "Pistachio", "Flopsy", "Little Foot", "Boots", "Cookie", "Honey Bunny", "Cottontail", "Oats", "Cuddles", "Hopper", "Pumpkin", "Shortbread", "Fluffy", "Pookie", "Heaven", "Koala", "Snickerdoodle", "Peanut" };

    private string[] cowNames = new string[] { "Bessie", "Clarabelle", "Betty Sue", "Emma", "Henrietta", "Ella", "Penelope", "Nettie", "Anna", "Bella", "Annabelle", "Dorothy", "Molly", "Gertie", "Annie" };
    [Header("Identity:")]
    public string animalName;
    public int animalAge;

    private void OnEnable()
    {
        if (health == 0) { health = 100; }
        if (water == 0) { water = 100; }
        healthReductionSpeed = Random.Range(1.0f, 2.0f);
        stats = FindObjectOfType<Stats>();
    }
    void Start()
    {
        meshRenderer.material = skins[Random.Range(0, skins.Length)];
        SetInfo();
        currentState = State.Wander;
        stats.animalCount += 1;
        UIManager.instance.UpdateValues();
        foodIndicator.SetActive(false);
        waterIndicator.SetActive(false);
        deathText = GameObject.Find("DeathText").GetComponent<Text>();

    }

    public void SetInfo()
    {
        if (animalType == "cat")
        {
            animalName = catNames[Random.Range(0, catNames.Length)];
            animalAge = Random.Range(1, 20);
        }

        if (animalType == "dog")
        {
            animalName = dogNames[Random.Range(0, dogNames.Length)];
            animalAge = Random.Range(1, 15);
        }

        if (animalType == "rabbit")
        {
            animalName = rabbitNames[Random.Range(0, rabbitNames.Length)];
            animalAge = Random.Range(1, 10);
        }
        if (animalType == "cow")
        {
            animalName = cowNames[Random.Range(0, cowNames.Length)];
            animalAge = Random.Range(1, 10);
        }
    }
    private void OnDestroy()
    {
        deathText.text = "";

    }
    // Update is called once per frame
    void Update()
    {
        health -= Time.deltaTime * healthReductionSpeed;
        water -= Time.deltaTime * (healthReductionSpeed / 2);
        if (health <= 0 || water <= 0)
        {
            if (!isDead)
            {
                stats.XP -= 75;
                stats.animalCount--;
                UIManager.instance.UpdateValues();
                currentState = State.Dead;
                health = 0;
                water = 0;
                Debug.Log(transform.name + " died");
                deathText.text = "Your " + animalType + " " + animalName + " died";
                animator.SetBool("isSleeping", true);
                agent.enabled = false;
            }
            isDead = true;
            if (rotateOnDeath)
            {
                if (transform.rotation.eulerAngles.z <= 90)
                {
                    transform.rotation *= Quaternion.Euler(0, 0, 90f * 2 * Time.deltaTime);
                }
            }
            Destroy(gameObject, 5);
            return;
        }
        if (health < 50)
        {
            foodIndicator.SetActive(true);
            currentState = State.goToFood;
        }
        else if (health >= 50)
        {
            foodIndicator.SetActive(false);
        }
        if (water < 50)
        {
            waterIndicator.SetActive(true);
            currentState = State.goToWater;

        }
        else if (water >= 50)
        {
            waterIndicator.SetActive(false);
        }
        if (currentState == State.Wander)
        {
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                target = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(target);
                timer = 0;
                wanderTimer += Random.Range(-3, 3);
                wanderTimer = Mathf.Clamp(wanderTimer, 5, 15);
            }
        }

        if (currentState == State.goToFood)
        {
            //Debug.Log(transform.name + " is hungry");

            if (findClosestResourceWithTag("Food") != null)
            {

                target = findClosestResourceWithTag("Food").transform.position;
                if (target != null)
                {
                    agent.SetDestination(target);
                    if (Vector3.Distance(transform.position, target) <= 2f)
                    {
                        Destroy(findClosestResourceWithTag("Food"));
                        findClosestResourceWithTag("Food").GetComponent<Resource>().isOccupied = true;
                        health = 100;
                        stats.XP += Random.Range(10, 20);
                        UIManager.instance.UpdateValues();
                        currentState = State.Wander;
                    }
                }
            }
            else
            {
                currentState = State.Wander;
            }
        }

        if (currentState == State.goToWater)
        {
            //Debug.Log(transform.name + " is thirsty");

            if (findClosestResourceWithTag("Water") != null)
            {

                target = findClosestResourceWithTag("Water").transform.position;
                if (target != null)
                {
                    agent.SetDestination(target);
                    if (Vector3.Distance(transform.position, target) <= 2f)
                    {
                        Destroy(findClosestResourceWithTag("Water"));
                        findClosestResourceWithTag("Water").GetComponent<Resource>().isOccupied = true;
                        water = 100;
                        stats.XP += Random.Range(10, 20);
                        UIManager.instance.UpdateValues();

                        currentState = State.Wander;
                    }
                }
            }
            else
            {
                currentState = State.Wander;
            }
        }

        if (agent.remainingDistance >= 3f)
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
            agent.speed = Mathf.Lerp(agent.speed, runSpeed, 2 * Time.deltaTime);
        }
        else if (agent.remainingDistance <= 0.2f)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
        else if (agent.remainingDistance < 3f)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", true);
            agent.speed = Mathf.Lerp(agent.speed, walkSpeed, 2 * Time.deltaTime);
        }
    }
    public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
    private GameObject findClosestResourceWithTag(string tagtoCheck)
    {

        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(tagtoCheck);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            if (!go.GetComponent<Resource>().isOccupied)
            {
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = go;
                    distance = curDistance;
                }
            }
        }

        return closest;
    }

    public void OnMouseDown()
    {
        cameraController.instance.followTransform = transform;
    }
}


