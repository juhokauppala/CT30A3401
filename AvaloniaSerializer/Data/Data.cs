using System;
using System.Xml.Serialization;
using MessagePack;


namespace AvaloniaSerializer.Data
{
    static class Data
    {
        public static object[] DataObjects = { new Data1(), new Data2(), new Data3() /*, new Data4() */ };
    }

    [MessagePackObject]
    [Serializable]
    public class Data1
    {
        [Key(0)]
        public string ObamaInfo = @"Barack Hussein Obama II (/bəˈrɑːk huːˈseɪn oʊˈbɑːmə/ (About this soundlisten);[1] born August 4, 1961) is an American attorney and politician who served as the 44th president of the United States from 2009 to 2017. A member of the Democratic Party, he was the first African-American president of the United States. He previously served as a U.S. senator from Illinois from 2005 to 2008 and an Illinois state senator from 1997 to 2004.
Obama was born in Honolulu, Hawaii.After graduating from Columbia University in 1983, he worked as a community organizer in Chicago.In 1988, he enrolled in Harvard Law School, where he was the first black person to head the Harvard Law Review.After graduating, he became a civil rights attorney and an academic, teaching constitutional law at the University of Chicago Law School from 1992 to 2004. Turning to elective politics, he represented the 13th district from 1997 until 2004 in the Illinois Senate, when he ran for the U.S.Senate.Obama received national attention in 2004 with his March Senate-primary win, his well-received July Democratic National Convention keynote address, and his landslide November election to the Senate. In 2008, he was nominated for president a year after his presidential campaign began, and after close primary campaigns against Hillary Clinton. Obama was elected over Republican John McCain and was inaugurated on January 20, 2009. Nine months later, he was named the 2009 Nobel Peace Prize laureate.
Obama signed many landmark bills into law during his first two years in office.The main reforms that were passed include the Patient Protection and Affordable Care Act (commonly referred to as the ""Affordable Care Act"" or ""Obamacare""), the Dodd–Frank Wall Street Reform and Consumer Protection Act, and the Don't Ask, Don't Tell Repeal Act of 2010. The American Recovery and Reinvestment Act of 2009 and Tax Relief, Unemployment Insurance Reauthorization, and Job Creation Act of 2010 served as economic stimulus amidst the Great Recession.After a lengthy debate over the national debt limit, he signed the Budget Control and the American Taxpayer Relief Acts.In foreign policy, he increased U.S.troop levels in Afghanistan, reduced nuclear weapons with the United States–Russia New START treaty, and ended military involvement in the Iraq War.He ordered military involvement in Libya, contributing to the overthrow of Muammar Gaddafi.He also ordered the military operations that resulted in the deaths of Osama bin Laden and suspected Yemeni Al-Qaeda operative Anwar al-Awlaki.
After winning re-election by defeating Republican opponent Mitt Romney, Obama was sworn in for a second term in 2013. During this term, he promoted inclusion for LGBT Americans. His administration filed briefs that urged the Supreme Court to strike down same-sex marriage bans as unconstitutional (United States v. Windsor and Obergefell v. Hodges); same-sex marriage was legalized nationwide in 2015 after the Court ruled so in Obergefell.He advocated for gun control in response to the Sandy Hook Elementary School shooting, indicating support for a ban on assault weapons, and issued wide-ranging executive actions concerning global warming and immigration.In foreign policy, he ordered military intervention in Iraq in response to gains made by ISIL after the 2011 withdrawal from Iraq, continued the process of ending U.S.combat operations in Afghanistan in 2016, promoted discussions that led to the 2015 Paris Agreement on global climate change, initiated sanctions against Russia following the invasion in Ukraine and again after Russian interference in the 2016 United States elections, brokered a nuclear deal with Iran, and normalized U.S.relations with Cuba.Obama nominated three justices to the Supreme Court: Sonia Sotomayor and Elena Kagan were confirmed as justices, while Merrick Garland faced unprecedented partisan obstruction and was ultimately not confirmed.During his term in office, America's reputation abroad significantly improved.[2]
Obama's presidency has generally been regarded favorably, and evaluations of his presidency among historians, political scientists, and the general public place him among the upper tier of American presidents. Obama left office and retired in January 2017, but continued to reside in Washington, D.C.[3][4] A December 2019 Gallup poll found that Obama was the most admired man in America for a record 12th consecutive year.[5]";

        [Key(1)]
        public string Presidency = @"The inauguration of Barack Obama as the 44th President took place on January 20, 2009. In his first few days in office, Obama issued executive orders and presidential memoranda directing the U.S. military to develop plans to withdraw troops from Iraq.[212] He ordered the closing of the Guantanamo Bay detention camp,[213] but Congress prevented the closure by refusing to appropriate the required funds[214][215][216] and preventing moving any Guantanamo detainee into the U.S. or to other countries.[217] Obama reduced the secrecy given to presidential records.[218] He also revoked President George W. Bush's restoration of President Ronald Reagan's Mexico City Policy prohibiting federal aid to international family planning organizations that perform or provide counseling about abortion.[219]";
    }

    [MessagePackObject]
    [Serializable]
    public class Data2
    {
        [Key(42)]
        public int Count = 42;
        [Key(0)]
        public double[] Weights = { 2.34241, 29387513.123897519, -123523.1531532, 1.5, -124124142124124.1241, 0.25911425628900475, 0.08620834410240841, 0.8519393868556824, 0.9558855559407629, 0.5543629691364149, 0.4107226210097934, 0.6154931156764001, 0.16358886534980188, 0.3937853354714693, 0.6039111426619372, 0.2967276226861373, 0.09915528076437807, 0.4713610607420682 };
        [Key(2)]
        public bool[] LifeChoices = { true, false, false, true, true };
    }

    [MessagePackObject]
    [Serializable]
    public class Data3
    {
        [Key(0)]
        public string a = "a";
        [Key(1)]
        public string b = "a";
        [Key(2)]
        public string c = "a";
        [Key(3)]
        public string d = "a";
        [Key(4)]
        public string e = "a";
        [Key(5)]
        public string f = "a";
        [Key(6)]
        public string g = "a";
        [Key(7)]
        public string h = "a";
        [Key(8)]
        public string i = "a";
        [Key(9)]
        public string j = "a";
        [Key(10)]
        public string k = "a";
        [Key(11)]
        public string l = "a";
        [Key(12)]
        public string m = "a";
        [Key(13)]
        public string n = "a";
        [Key(14)]
        public string o = "a";
        [Key(15)]
        public string p = "a";
        [Key(16)]
        public string q = "a";
        [Key(17)]
        public string r = "a";
        [Key(18)]
        public string s = "a";
        [Key(19)]
        public string t = "a";
        [Key(20)]
        public string u = "a";
        [Key(21)]
        public string v = "a";
        [Key(22)]
        public string w = "a";
        [Key(23)]
        public string x = "a";
        [Key(24)]
        public string y = "a";
        [Key(25)]
        public string z = "a";
        [Key(26)]
        public bool _a = false;
        [Key(27)]
        public int _b = 200297;
        [Key(28)]
        public int _c = 0506701;
        [Key(29)]
        public string _d = "a";
        [Key(30)]
        public string _e = "a";
    }
    /* Include this to test scalability of histogram UI
    [MessagePackObject]
    [Serializable]
    public class Data4
    {
        [Key(0)]
        public bool ThisWorksOnDocker = true;
        [Key(1)]
        public string HowSo = "I hope because I tested 2 frameworks that didn't";
    }
    */
}
