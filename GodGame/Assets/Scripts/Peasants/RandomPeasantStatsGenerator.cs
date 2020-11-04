using System.Collections;
using System.Collections.Generic;
//using TMPro;
using UnityEngine;
//using UnityEngine.UI;


public struct PeasantStats
{
    public int health;
    public bool isMale;
    public Ages age;
    public float walkingSpeed;
    public float runningSpeed;
    public string name;
    //public House House { get; set; }//house and job assigned by buildingmanager? because dependeon on expanding and contracting lists of those
    //public Jobs Job { get; set; }
}

public static class LocomotionSpeeds
{
    public static float AdultWalk = 3.5f;
    public static float AdultRun = 5f;
    public static float ChildWalk = 4f;
    public static float ChildRun = 5.5f;
    public static float OldWalk = 2.5f;
    public static float OldRun = 4f;
}

public static class HealthValues
{
    public static int AdultHealth = 4;
    public static int ChildHealth = 3;
    public static int OldHealth = 2;
}

public class RandomPeasantStatsGenerator : MonoBehaviour
{
    public static int totalDeadEnemies;
    [SerializeField]
    private int numRandomNamesToGenerate;
    //bool hasGeneratedNames = false;
    //bool hasFinishedGeneratingNames = false;
    string[] firstMaleNamesArray;
    string[] firstFemaleNamesArray;
    string[] lastNamesArray;
    string[] middleMaleNamesAndAcronymsArray;
    string[] middleFemaleNamesAndAcronymsArray;

    string[] finalMaleNames;
    string[] finalFemaleNames;

    int maleNameIndex = 0;
    int femaleNameIndex = 0;

    //[SerializeField]
    //TextMeshProUGUI namesListDisplayPro;
    //[SerializeField]
    //TextMeshProUGUI namesListDisplayPro2;
    //[SerializeField]
    //TextMeshProUGUI namesListDisplayPro3;
    //[SerializeField]
    //TextMeshProUGUI namesListDisplayPro4;

    //GameObject[] rosterUIObjects;


    // awake is called before starts so use it to setup references
    void Awake()
    {
        finalMaleNames = new string[numRandomNamesToGenerate];
        finalFemaleNames = new string[numRandomNamesToGenerate];

        firstMaleNamesArray = maleFirstNames.Split(',');
        firstFemaleNamesArray = femaleFirstNames.Split(',');
        lastNamesArray = lastNames.Split(',');
        middleMaleNamesAndAcronymsArray = maleMiddleNames.Split(',');
        middleFemaleNamesAndAcronymsArray = femaleMiddleNames.Split(',');
        GenerateNames();
        //rosterUIObjects = GameObject.FindGameObjectsWithTag("DeathRoster");
        //for (int i = 0; i < rosterUIObjects.Length; i++)
        //{
        //    rosterUIObjects[i].SetActive(false);
        //}
    }

    public PeasantStats GenerateNewPeasantStats()
    {
        PeasantStats stats = new PeasantStats();
        int oddsOfBeingMale = Random.Range(0, 2);
        bool isMale = false;
        if (oddsOfBeingMale == 1)
        {
            isMale = true;
        }
        stats.isMale = isMale;

        if (isMale)
        {
            int randomCompleteMaleNameIndex = Random.Range(0, /*finalMaleNames.Length*/maleNameIndex);//only use about half the space provided
            stats.name = finalMaleNames[randomCompleteMaleNameIndex];
        }
        else
        {
            int randomCompleteFemaleNameIndex = Random.Range(0, /*finalFemaleNames.Length*/femaleNameIndex);//same
            stats.name = finalFemaleNames[randomCompleteFemaleNameIndex];
        }

        int ageOdds = Random.Range(0, 5);
        int speedmodifierOdds = Random.Range(-1, 2);
        float randomSpeedModifier = speedmodifierOdds * 0.5f;
        if(ageOdds == 0)
        {
            stats.age = Ages.Child;
            stats.runningSpeed = LocomotionSpeeds.ChildRun + randomSpeedModifier;
            stats.walkingSpeed = LocomotionSpeeds.ChildWalk + randomSpeedModifier;
            stats.health = HealthValues.ChildHealth;
        }
        else if(ageOdds == 4)
        {
            stats.age = Ages.Old;
            stats.runningSpeed = LocomotionSpeeds.OldRun + randomSpeedModifier;
            stats.walkingSpeed = LocomotionSpeeds.OldWalk + randomSpeedModifier;
            stats.health = HealthValues.OldHealth;
        }
        else
        {
            stats.age = Ages.Adult;
            stats.runningSpeed = LocomotionSpeeds.AdultRun + randomSpeedModifier;
            stats.walkingSpeed = LocomotionSpeeds.AdultWalk + randomSpeedModifier;
            stats.health = HealthValues.AdultHealth;
        }

        return stats;
    }

    void GenerateNames()
    {
        

        for (int i = 0; i < numRandomNamesToGenerate; i++)
        {
            int oddsOfBeingMale = Random.Range(0, 2);
            bool isMale = false;
            if (oddsOfBeingMale == 1)
            {
                isMale = true;
            }

            string name = "";
            if (isMale)
            {
                int randomMaleFirstNameIndex = Random.Range(0, firstMaleNamesArray.Length);
                int randomLastNameIndex = Random.Range(0, lastNamesArray.Length);
                int randomMaleMiddleNameIndex = -1;
                int oddsOfHavingMiddleName = Random.Range(0, 2);
                if (oddsOfHavingMiddleName == 1)
                {
                    randomMaleMiddleNameIndex = Random.Range(0, middleMaleNamesAndAcronymsArray.Length);
                }

                if (randomMaleMiddleNameIndex == -1)
                {
                    name = firstMaleNamesArray[randomMaleFirstNameIndex] + " " + lastNamesArray[randomLastNameIndex];
                }
                else
                {
                    name = firstMaleNamesArray[randomMaleFirstNameIndex] + " " + middleMaleNamesAndAcronymsArray[randomMaleMiddleNameIndex] + " " + lastNamesArray[randomLastNameIndex];
                }

                finalMaleNames[maleNameIndex] = name;
                maleNameIndex++;
            }
            else
            {
                int randomFemaleFirstNameIndex = Random.Range(0, firstFemaleNamesArray.Length);
                int randomLastNameIndex = Random.Range(0, lastNamesArray.Length);
                int randomFemaleMiddleNameIndex = -1;
                int oddsOfHavingMiddleName = Random.Range(0, 2);
                if (oddsOfHavingMiddleName == 1)
                {
                    randomFemaleMiddleNameIndex = Random.Range(0, middleFemaleNamesAndAcronymsArray.Length);
                }

                if (randomFemaleMiddleNameIndex == -1)
                {
                    name = firstFemaleNamesArray[randomFemaleFirstNameIndex] + " " + lastNamesArray[randomLastNameIndex];
                }
                else
                {
                    name = firstFemaleNamesArray[randomFemaleFirstNameIndex] + " " + middleFemaleNamesAndAcronymsArray[randomFemaleMiddleNameIndex] + " " + lastNamesArray[randomLastNameIndex];
                }

                finalFemaleNames[femaleNameIndex] = name;
                femaleNameIndex++;
            }

        }
        //hasFinishedGeneratingNames = true;
    }
    //bool hasDisplayedNames = false;
    // Update is called once per frame
    void Update()
    {
        //if (!hasGeneratedNames && numRandomNamesToGenerate != 0)
        //{
        //    ProgressBar.maximum = numRandomNamesToGenerate;
        //    hasGeneratedNames = true;
        //    finalMaleNames = new string[numRandomNamesToGenerate];
        //    GenerateNames();
        //}

        //if (hasFinishedGeneratingNames)
        //{

        //}

        //int diff = numRandomNamesToGenerate - totalDeadEnemies;
        //if (diff <= 15 && !hasDisplayedNames && hasFinishedGeneratingNames)
        //{
        //    for (int i = 0; i < rosterUIObjects.Length; i++)
        //    {
        //        rosterUIObjects[i].SetActive(true);//this will block input, which i think is good for the end screen

        //        //if (i == (rosterUIObjects.Length + 1))
        //        //{
        //        //    hasDisplayedNames = true;
        //        //}
        //    }
        //    //DisplayNames();

        //    //if 5 or less enemies left
        //    //display end game screen
        //}
    }

    //private void DisplayNames()
    //{
    //    int numPerList = totalEnemies / 4;
    //    for (int i = 0; i < numPerList; i++)
    //    {
    //        namesListDisplayPro.text += finalNames[i] + '\n';
    //    }

    //    for (int i = numPerList; i < numPerList * 2; i++)
    //    {
    //        namesListDisplayPro2.text += finalNames[i] + '\n';
    //    }

    //    for (int i = numPerList * 2; i < numPerList * 3; i++)
    //    {
    //        namesListDisplayPro3.text += finalNames[i] + '\n';
    //    }

    //    for (int i = numPerList * 3; i < numPerList * 4; i++)
    //    {
    //        namesListDisplayPro4.text += finalNames[i] + '\n';
    //    }
    //    hasDisplayedNames = true;
    //}

    private string lastNames = "SMITH,JOHNSON,WILLIAMS,JONES,BROWN,DAVIS,MILLER,WILSON,MOORE,TAYLOR,ANDERSON,THOMAS,JACKSON,WHITE,HARRIS,MARTIN,THOMPSON,GARCIA,MARTINEZ,ROBINSON,CLARK,RODRIGUEZ,LEWIS,LEE,WALKER,HALL,ALLEN,YOUNG,HERNANDEZ,KING,WRIGHT,LOPEZ,HILL,SCOTT,GREEN,ADAMS,BAKER,GONZALEZ,NELSON,CARTER,MITCHELL,PEREZ,ROBERTS,TURNER,PHILLIPS,CAMPBELL,PARKER,EVANS,EDWARDS,COLLINS,STEWART,SANCHEZ,MORRIS,ROGERS,REED,COOK,MORGAN,BELL,MURPHY,BAILEY,RIVERA,COOPER,RICHARDSON,COX,HOWARD,WARD,TORRES,PETERSON,GRAY,RAMIREZ,JAMES,WATSON,BROOKS,KELLY,SANDERS,PRICE,BENNETT,WOOD,BARNES,ROSS,HENDERSON,COLEMAN,JENKINS,PERRY,POWELL,LONG,PATTERSON,HUGHES,FLORES,WASHINGTON,BUTLER,SIMMONS,FOSTER,GONZALES,BRYANT,ALEXANDER,RUSSELL,GRIFFIN,DIAZ,HAYES,MYERS,FORD,HAMILTON,GRAHAM,SULLIVAN,WALLACE,WOODS,COLE,WEST,JORDAN,OWENS,REYNOLDS,FISHER,ELLIS,HARRISON,GIBSON,MCDONALD,CRUZ,MARSHALL,ORTIZ,GOMEZ,MURRAY,FREEMAN,WELLS,WEBB,SIMPSON,STEVENS,TUCKER,PORTER,HUNTER,HICKS,CRAWFORD,HENRY,BOYD,MASON,MORALES,KENNEDY,WARREN,DIXON,RAMOS,REYES,BURNS,GORDON,SHAW,HOLMES,RICE,ROBERTSON,HUNT,BLACK,DANIELS,PALMER,MILLS,NICHOLS,GRANT,KNIGHT,FERGUSON,ROSE,STONE,HAWKINS,DUNN,PERKINS,HUDSON,SPENCER,GARDNER,STEPHENS,PAYNE,PIERCE,BERRY,MATTHEWS,ARNOLD,WAGNER,WILLIS,RAY,WATKINS,OLSON,CARROLL,DUNCAN,SNYDER,HART,CUNNINGHAM,BRADLEY,LANE,ANDREWS,RUIZ,HARPER,FOX,RILEY,ARMSTRONG,CARPENTER,WEAVER,GREENE,LAWRENCE,ELLIOTT,CHAVEZ,SIMS,AUSTIN,PETERS,KELLEY,FRANKLIN,LAWSON,FIELDS,GUTIERREZ,RYAN,SCHMIDT,CARR,VASQUEZ,CASTILLO,WHEELER,CHAPMAN,OLIVER,MONTGOMERY,RICHARDS,WILLIAMSON,JOHNSTON,BANKS,MEYER,BISHOP,MCCOY,HOWELL,ALVAREZ,MORRISON,HANSEN,FERNANDEZ,GARZA,HARVEY,LITTLE,BURTON,STANLEY,NGUYEN,GEORGE,JACOBS,REID,KIM,FULLER,LYNCH,DEAN,GILBERT,GARRETT,ROMERO,WELCH,LARSON,FRAZIER,BURKE,HANSON,DAY,MENDOZA,MORENO,BOWMAN,MEDINA,FOWLER,BREWER,HOFFMAN,CARLSON,SILVA,PEARSON,HOLLAND,DOUGLAS,FLEMING,JENSEN,VARGAS,BYRD,DAVIDSON,HOPKINS,MAY,TERRY,HERRERA,WADE,SOTO,WALTERS,CURTIS,NEAL,CALDWELL,LOWE,JENNINGS,BARNETT,GRAVES,JIMENEZ,HORTON,SHELTON,BARRETT,OBRIEN,CASTRO,SUTTON,GREGORY,MCKINNEY,LUCAS,MILES,CRAIG,RODRIQUEZ,CHAMBERS,HOLT,LAMBERT,FLETCHER,WATTS,BATES,HALE,RHODES,PENA,BECK,NEWMAN,HAYNES,MCDANIEL,MENDEZ,BUSH,VAUGHN,PARKS,DAWSON,SANTIAGO,NORRIS,HARDY,LOVE,STEELE,CURRY,POWERS,SCHULTZ,BARKER,GUZMAN,PAGE,MUNOZ,BALL,KELLER,";
    private string maleFirstNames = "JAMES,JOHN,ROBERT,MICHAEL,WILLIAM,DAVID,RICHARD,CHARLES,JOSEPH,THOMAS,CHRISTOPHER,DANIEL,PAUL,MARK,DONALD,GEORGE,KENNETH,STEVEN,EDWARD,BRIAN,RONALD,ANTHONY,KEVIN,JASON,MATTHEW,GARY,TIMOTHY,JOSE,LARRY,JEFFREY,FRANK,SCOTT,ERIC,STEPHEN,ANDREW,RAYMOND,GREGORY,JOSHUA,JERRY,DENNIS,WALTER,PATRICK,PETER,HAROLD,DOUGLAS,HENRY,CARL,ARTHUR,RYAN,ROGER,JOE,JUAN,JACK,ALBERT,JONATHAN,JUSTIN,TERRY,GERALD,KEITH,SAMUEL,WILLIE,RALPH,LAWRENCE,NICHOLAS,ROY,BENJAMIN,BRUCE,BRANDON,ADAM,HARRY,FRED,WAYNE,BILLY,STEVE,LOUIS,JEREMY,AARON,RANDY,HOWARD,EUGENE,CARLOS,RUSSELL,BOBBY,VICTOR,MARTIN,ERNEST,PHILLIP,TODD,JESSE,CRAIG,ALAN,SHAWN,CLARENCE,SEAN,PHILIP,CHRIS,JOHNNY,EARL,JIMMY,ANTONIO,DANNY,BRYAN,TONY,LUIS,MIKE,STANLEY,LEONARD,NATHAN,DALE,MANUEL,RODNEY,CURTIS,NORMAN,ALLEN,MARVIN,VINCENT,GLENN,JEFFERY,TRAVIS,JEFF,CHAD,JACOB,LEE,MELVIN,ALFRED,KYLE,FRANCIS,BRADLEY,JESUS,HERBERT,FREDERICK,RAY,JOEL,EDWIN,DON,EDDIE,RICKY,TROY,RANDALL,BARRY,ALEXANDER,BERNARD,MARIO,LEROY,FRANCISCO,MARCUS,MICHEAL,THEODORE,CLIFFORD,MIGUEL,OSCAR,JAY,JIM,TOM,CALVIN,ALEX,JON,RONNIE,BILL,LLOYD,TOMMY,LEON,DEREK,WARREN,DARRELL,JEROME,FLOYD,LEO,ALVIN,TIM,WESLEY,GORDON,DEAN,GREG,JORGE,DUSTIN,PEDRO,DERRICK,DAN,LEWIS,ZACHARY,COREY,HERMAN,MAURICE,VERNON,ROBERTO,CLYDE,GLEN,HECTOR,SHANE,RICARDO,SAM,RICK,LESTER,BRENT,RAMON,CHARLIE,TYLER,GILBERT,GENE,MARC,REGINALD,RUBEN,BRETT,ANGEL,NATHANIEL,RAFAEL,LESLIE,EDGAR,MILTON,RAUL,BEN,CHESTER,CECIL,DUANE,FRANKLIN,ANDRE,ELMER,BRAD,GABRIEL,RON,MITCHELL,ROLAND,ARNOLD,HARVEY,JARED,ADRIAN,KARL,CORY,CLAUDE,ERIK,DARRYL,JAMIE,NEIL,JESSIE,CHRISTIAN,JAVIER,FERNANDO,CLINTON,TED,MATHEW,TYRONE,DARREN,LONNIE,LANCE,CODY,JULIO,KELLY,KURT,ALLAN,NELSON,GUY,CLAYTON,HUGH,MAX,DWAYNE,DWIGHT,ARMANDO,FELIX,JIMMIE,EVERETT,JORDAN,IAN,WALLACE,KEN,BOB,JAIME,CASEY,ALFREDO,ALBERTO,DAVE,IVAN,JOHNNIE,SIDNEY,BYRON,JULIAN,ISAAC,MORRIS,CLIFTON,WILLARD,DARYL,ROSS,VIRGIL,ANDY,MARSHALL,SALVADOR,PERRY,KIRK,SERGIO,MARION,TRACY,SETH,KENT,TERRANCE,RENE,EDUARDO,TERRENCE,ENRIQUE,FREDDIE,WADE,HAYNES,MCDANIEL,MENDEZ,";
    private string femaleFirstNames = "PAGE,MARY,PATRICIA,LINDA,BARBARA,ELIZABETH,JENNIFER,MARIA,SUSAN,MARGARET,DOROTHY,LISA,NANCY,KAREN,BETTY,HELEN,SANDRA,DONNA,CAROL,RUTH,SHARON,MICHELLE,LAURA,SARAH,KIMBERLY,DEBORAH,JESSICA,SHIRLEY,CYNTHIA,ANGELA,MELISSA,BRENDA,AMY,ANNA,REBECCA,VIRGINIA,KATHLEEN,PAMELA,MARTHA,DEBRA,AMANDA,STEPHANIE,CAROLYN,CHRISTINE,MARIE,JANET,CATHERINE,FRANCES,ANN,JOYCE,DIANE,ALICE,JULIE,HEATHER,TERESA,DORIS,GLORIA,EVELYN,JEAN,CHERYL,MILDRED,KATHERINE,JOAN,ASHLEY,JUDITH,ROSE,JANICE,KELLY,NICOLE,JUDY,CHRISTINA,KATHY,THERESA,BEVERLY,DENISE,TAMMY,IRENE,JANE,LORI,RACHEL,MARILYN,ANDREA,KATHRYN,LOUISE,SARA,ANNE,JACQUELINE,WANDA,BONNIE,JULIA,RUBY,LOIS,TINA,PHYLLIS,NORMA,PAULA,DIANA,ANNIE,LILLIAN,EMILY,ROBIN,PEGGY,CRYSTAL,GLADYS,RITA,DAWN,CONNIE,FLORENCE,TRACY,EDNA,TIFFANY,CARMEN,ROSA,CINDY,GRACE,WENDY,VICTORIA,EDITH,KIM,SHERRY,SYLVIA,JOSEPHINE,THELMA,SHANNON,SHEILA,ETHEL,ELLEN,ELAINE,MARJORIE,CARRIE,CHARLOTTE,MONICA,ESTHER,PAULINE,EMMA,JUANITA,ANITA,RHONDA,HAZEL,AMBER,EVA,DEBBIE,APRIL,LESLIE,CLARA,LUCILLE,JAMIE,JOANNE,ELEANOR,VALERIE,DANIELLE,MEGAN,ALICIA,SUZANNE,MICHELE,GAIL,BERTHA,DARLENE,VERONICA,JILL,ERIN,GERALDINE,LAUREN,CATHY,JOANN,LORRAINE,LYNN,SALLY,REGINA,ERICA,BEATRICE,DOLORES,BERNICE,AUDREY,YVONNE,ANNETTE,JUNE,SAMANTHA,MARION,DANA,STACY,ANA,RENEE,IDA,VIVIAN,ROBERTA,HOLLY,BRITTANY,MELANIE,LORETTA,YOLANDA,JEANETTE,LAURIE,KATIE,KRISTEN,VANESSA,ALMA,SUE,ELSIE,BETH,JEANNE,VICKI,CARLA,TARA,ROSEMARY,EILEEN,TERRI,GERTRUDE,LUCY,TONYA,ELLA,STACEY,WILMA,GINA,KRISTIN,JESSIE,NATALIE,AGNES,VERA,WILLIE,CHARLENE,BESSIE,DELORES,MELINDA,PEARL,ARLENE,MAUREEN,COLLEEN,ALLISON,TAMARA,JOY,GEORGIA,CONSTANCE,LILLIE,CLAUDIA,JACKIE,MARCIA,TANYA,NELLIE,MINNIE,MARLENE,HEIDI,GLENDA,LYDIA,VIOLA,COURTNEY,MARIAN,STELLA,CAROLINE,DORA,JO,VICKIE,MATTIE,TERRY,MAXINE,IRMA,MABEL,MARSHA,MYRTLE,LENA,CHRISTY,DEANNA,PATSY,HILDA,GWENDOLYN,JENNIE,NORA,MARGIE,NINA,CASSANDRA,LEAH,PENNY,KAY,PRISCILLA,NAOMI,CAROLE,BRANDY,OLGA,BILLIE,DIANNE,TRACEY,LEONA,JENNY,FELICIA,SONIA,MIRIAM,VELMA,BECKY,BOBBIE,VIOLET,KRISTINA,TONI,MISTY,MAE,SHELLY,DAISY,RAMONA,SHERRI,ERIKA,KATRINA,CLAIRE,LINDSEY,LINDSAY,GENEVA,GUADALUPE,BELINDA,MARGARITA,SHERYL,CORA,FAYE,ADA,NATASHA,SABRINA,ISABEL,MARGUERITE,HATTIE,HARRIET,MOLLY,CECILIA,KRISTI,BRANDI,BLANCHE,SANDY,ROSIE,JOANNA,IRIS,EUNICE,ANGIE,INEZ,LYNDA,MADELINE,AMELIA,ALBERTA,GENEVIEVE,MONIQUE,JODI,JANIE,MAGGIE,KAYLA,SONYA,JAN,LEE,KRISTINE,CANDACE,FANNIE,MARYANN,OPAL,ALISON,YVETTE,MELODY,LUZ,SUSIE,OLIVIA,FLORA,SHELLEY,KRISTY,MAMIE,LULA,LOLA,VERNA,BEULAH,ANTOINETTE,CANDICE,JUANA,JEANNETTE,PAM,KELLI,HANNAH,WHITNEY,BRIDGET,KARLA,CELIA,LATOYA,PATTY,SHELIA,GAYLE,DELLA,VICKY,LYNNE,SHERI,MARIANNE,KARA,JACQUELYN,ERMA,BLANCA,MYRA,LETICIA,PAT,KRISTA,ROXANNE,ANGELICA,JOHNNIE,ROBYN,FRANCIS,ADRIENNE,ROSALIE,ALEXANDRA,BROOKE,BETHANY,SADIE,BERNADETTE,TRACI,JODY,KENDRA,JASMINE,NICHOLE,RACHAEL,CHELSEA,MABLE,ERNESTINE,MURIEL,MARCELLA,ELENA,KRYSTAL,ANGELINA,NADINE,KARI,ESTELLE,DIANNA,PAULETTE,LORA,MONA,DOREEN,ROSEMARIE,ANGEL,DESIREE,ANTONIA,HOPE,GINGER,JANIS,BETSY,CHRISTIE,FREDA,MERCEDES,MEREDITH,LYNETTE,TERI,CRISTINA,EULA,LEIGH,MEGHAN,SOPHIA,ELOISE,ROCHELLE,GRETCHEN,CECELIA,RAQUEL,HENRIETTA,ALYSSA,JANA,KELLEY,GWEN,KERRY,JENNA,TRICIA,LAVERNE,OLIVE,ALEXIS,TASHA,SILVIA,ELVIRA,CASEY,DELIA,SOPHIE,KATE,PATTI,LORENA,KELLIE,SONJA,LILA,LANA,DARLA,MAY,MINDY,ESSIE,MANDY,LORENE,ELSA,JOSEFINA,JEANNIE,MIRANDA,DIXIE,LUCIA,MARTA,FAITH,LELA,JOHANNA,SHARI,CAMILLE,TAMI,SHAWNA,ELISA,EBONY,MELBA,ORA,NETTIE,TABITHA,OLLIE,JAIME,WINIFRED,KRISTIE,MARINA,ALISHA,AIMEE,RENA,MYRNA,MARLA,TAMMIE,LATASHA,BONITA,PATRICE,RONDA,SHERRIE,ADDIE,FRANCINE,DELORIS,STACIE,ADRIANA,CHERI,SHELBY,ABIGAIL,CELESTE,JEWEL,CARA,ADELE,REBEKAH,LUCINDA,DORTHY,CHRIS,EFFIE,TRINA,REBA,SHAWN,SALLIE,AURORA,";


    private string maleMiddleNames = "DAVIS,MARTINEZ,THOMAS,CHRISTOPHER,DANIEL,PAUL,MARK,K.,M.,R.R.,L.,T,H.,A.,U.,P.,B.,C.,D.,E.,E.E.,F.,H.,J.,L.K.,";
    private string femaleMiddleNames = "DAVIS,MARTINEZ,JEANNIE,MIRANDA,DIXIE,LUCIA,MARTA,FAITH,LELA,K.,M.,R.R.,L.,T,H.,A.,U.,P.,B.,C.,D.,E.,E.E.,F.,H.,J.,L.K.,";

}
