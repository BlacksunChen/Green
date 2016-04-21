 /*//DELETE THIS LINE FOR SOOMLA
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Store
{
	public class my_Soomla_Assets : IStoreAssets{
		
		public int GetVersion()
		{
			return 0;
		}
		
		#region virtual currency

		public const string prefix = "com_myName_myApp_";

		//if you don't need this, leave it empty, BUT NOT delete it!
		public  VirtualCurrency[] GetCurrencies()//virtual money
		{
			return new VirtualCurrency[]{VIRTUAL_MONEY_PROFILE_0,
										VIRTUAL_MONEY_PROFILE_1,
										VIRTUAL_MONEY_PROFILE_2,
										VIRTUAL_MONEY_PROFILE_3,
										VIRTUAL_MONEY_PROFILE_4,
										VIRTUAL_MONEY_PROFILE_5,
										VIRTUAL_MONEY_PROFILE_6,
										VIRTUAL_MONEY_PROFILE_7,
										VIRTUAL_MONEY_PROFILE_8,
										VIRTUAL_MONEY_PROFILE_9
										
			};
		}

		public const string VIRTUAL_MONEY_PROFILE_0_ID    	= prefix+"virtual_money_p0";
		public const string VIRTUAL_MONEY_PROFILE_1_ID      = prefix+"virtual_money_p1";
		public const string VIRTUAL_MONEY_PROFILE_2_ID      = prefix+"virtual_money_p2";
		public const string VIRTUAL_MONEY_PROFILE_3_ID      = prefix+"virtual_money_p3";
		public const string VIRTUAL_MONEY_PROFILE_4_ID      = prefix+"virtual_money_p4";
		public const string VIRTUAL_MONEY_PROFILE_5_ID      = prefix+"virtual_money_p5";
		public const string VIRTUAL_MONEY_PROFILE_6_ID      = prefix+"virtual_money_p6";
		public const string VIRTUAL_MONEY_PROFILE_7_ID      = prefix+"virtual_money_p7";
		public const string VIRTUAL_MONEY_PROFILE_8_ID      = prefix+"virtual_money_p8";
		public const string VIRTUAL_MONEY_PROFILE_9_ID      = prefix+"virtual_money_p9";


		const string virtual_money_name = "Coins";
		const string virtual_money_description = "";

		public static VirtualCurrency VIRTUAL_MONEY_PROFILE_0 = new VirtualCurrency(
			virtual_money_name,								// name
			virtual_money_description,						// description
			VIRTUAL_MONEY_PROFILE_0_ID						// item id
			);
		
		public static VirtualCurrency VIRTUAL_MONEY_PROFILE_1 = new VirtualCurrency(
			virtual_money_name,								// name
			virtual_money_description,						// description
			VIRTUAL_MONEY_PROFILE_1_ID						// item id
			);
		
		public static VirtualCurrency VIRTUAL_MONEY_PROFILE_2 = new VirtualCurrency(
			virtual_money_name,								// name
			virtual_money_description,						// description
			VIRTUAL_MONEY_PROFILE_2_ID						// item id
			);
		
		public static VirtualCurrency VIRTUAL_MONEY_PROFILE_3 = new VirtualCurrency(
			virtual_money_name,								// name
			virtual_money_description,						// description
			VIRTUAL_MONEY_PROFILE_3_ID						// item id
			);

		public static VirtualCurrency VIRTUAL_MONEY_PROFILE_4 = new VirtualCurrency(
			virtual_money_name,								// name
			virtual_money_description,						// description
			VIRTUAL_MONEY_PROFILE_4_ID						// item id
			);

		public static VirtualCurrency VIRTUAL_MONEY_PROFILE_5 = new VirtualCurrency(
			virtual_money_name,								// name
			virtual_money_description,						// description
			VIRTUAL_MONEY_PROFILE_5_ID						// item id
			);

		public static VirtualCurrency VIRTUAL_MONEY_PROFILE_6 = new VirtualCurrency(
			virtual_money_name,								// name
			virtual_money_description,						// description
			VIRTUAL_MONEY_PROFILE_6_ID						// item id
			);
		
		public static VirtualCurrency VIRTUAL_MONEY_PROFILE_7 = new VirtualCurrency(
			virtual_money_name,								// name
			virtual_money_description,						// description
			VIRTUAL_MONEY_PROFILE_7_ID						// item id
			);
		
		public static VirtualCurrency VIRTUAL_MONEY_PROFILE_8 = new VirtualCurrency(
			virtual_money_name,								// name
			virtual_money_description,						// description
			VIRTUAL_MONEY_PROFILE_8_ID						// item id
			);
		
		public static VirtualCurrency VIRTUAL_MONEY_PROFILE_9 = new VirtualCurrency(
			virtual_money_name,								// name
			virtual_money_description,						// description
			VIRTUAL_MONEY_PROFILE_9_ID						// item id
			);
		

		
		
		//if you don't need this, leave it empty, BUT NOT delete it!
		public VirtualCurrencyPack[] GetCurrencyPacks()//buy virtual money with real money
		{
			return new VirtualCurrencyPack[]{VIRTUAL_MONEY_PROFILE_0_PACK_10,VIRTUAL_MONEY_PROFILE_0_PACK_20,VIRTUAL_MONEY_PROFILE_0_PACK_50,
											VIRTUAL_MONEY_PROFILE_1_PACK_10,VIRTUAL_MONEY_PROFILE_1_PACK_20,VIRTUAL_MONEY_PROFILE_1_PACK_50,
											VIRTUAL_MONEY_PROFILE_2_PACK_10,VIRTUAL_MONEY_PROFILE_2_PACK_20,VIRTUAL_MONEY_PROFILE_2_PACK_50,
											VIRTUAL_MONEY_PROFILE_3_PACK_10,VIRTUAL_MONEY_PROFILE_3_PACK_20,VIRTUAL_MONEY_PROFILE_3_PACK_50,
											VIRTUAL_MONEY_PROFILE_4_PACK_10,VIRTUAL_MONEY_PROFILE_4_PACK_20,VIRTUAL_MONEY_PROFILE_4_PACK_50,
											VIRTUAL_MONEY_PROFILE_5_PACK_10,VIRTUAL_MONEY_PROFILE_5_PACK_20,VIRTUAL_MONEY_PROFILE_5_PACK_50,
											VIRTUAL_MONEY_PROFILE_6_PACK_10,VIRTUAL_MONEY_PROFILE_6_PACK_20,VIRTUAL_MONEY_PROFILE_6_PACK_50,
											VIRTUAL_MONEY_PROFILE_7_PACK_10,VIRTUAL_MONEY_PROFILE_7_PACK_20,VIRTUAL_MONEY_PROFILE_7_PACK_50,
											VIRTUAL_MONEY_PROFILE_8_PACK_10,VIRTUAL_MONEY_PROFILE_8_PACK_20,VIRTUAL_MONEY_PROFILE_8_PACK_50,
											VIRTUAL_MONEY_PROFILE_9_PACK_10,VIRTUAL_MONEY_PROFILE_9_PACK_20,VIRTUAL_MONEY_PROFILE_9_PACK_50
											
											};
		}

		const string pack10_name = "10 Coins";
		const string pack10_description = "get 10 coins";
		const int pack10_quantity = 10;
		const double pack10_cost = 0.99;

		const string pack20_name = "20 Coins";
		const string pack20_description = "get 20 coins";
		const int pack20_quantity = 20;
		const double pack20_cost = 2.99;

		const string pack50_name = "50 Coins";
		const string pack50_description = "get 50 coins";
		const int pack50_quantity = 50;
		const double pack50_cost = 5.99;

		//profile 0
		public const string VIRTUAL_MONEY_PROFILE_0_PACK_10_ID      = prefix+"virtual_money_p0_pack_10";
		public const string VIRTUAL_MONEY_PROFILE_0_PACK_20_ID      = prefix+"virtual_money_p0_pack_20";
		public const string VIRTUAL_MONEY_PROFILE_0_PACK_50_ID      = prefix+"virtual_money_p0_pack_50";
		
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_0_PACK_10 = new VirtualCurrencyPack(
			pack10_name,                                   	// name
			pack10_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_0_PACK_10_ID,                     // item id
			pack10_quantity,												// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_0_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_0_PACK_10_ID, pack10_cost)
			);
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_0_PACK_20 = new VirtualCurrencyPack(
			pack20_name,                                   	// name
			pack20_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_0_PACK_20_ID,                     // item id
			pack20_quantity,												// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_0_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_0_PACK_20_ID, pack20_cost)
			);
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_0_PACK_50 = new VirtualCurrencyPack(
			pack50_name,                                   	// name
			pack50_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_0_PACK_50_ID,                     // item id
			pack50_quantity,												// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_0_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_0_PACK_50_ID, pack50_cost)
			);
		
		//profile 1
		public const string VIRTUAL_MONEY_PROFILE_1_PACK_10_ID      = prefix+"virtual_money_p1_pack_10";
		public const string VIRTUAL_MONEY_PROFILE_1_PACK_20_ID      = prefix+"virtual_money_p1_pack_20";
		public const string VIRTUAL_MONEY_PROFILE_1_PACK_50_ID      = prefix+"virtual_money_p1_pack_50";
		
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_1_PACK_10 = new VirtualCurrencyPack(
			pack10_name,                                   	// name
			pack10_description,                      	 	// description
			VIRTUAL_MONEY_PROFILE_1_PACK_10_ID,             // item id
			pack10_quantity,								// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_1_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_1_PACK_10_ID, pack10_cost)
			);
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_1_PACK_20 = new VirtualCurrencyPack(
			pack20_name,                                   	// name
			pack20_description,                      	 	// description
			VIRTUAL_MONEY_PROFILE_1_PACK_20_ID,             // item id
			pack20_quantity,								// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_1_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_1_PACK_20_ID, pack20_cost)
			);
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_1_PACK_50 = new VirtualCurrencyPack(
			pack50_name,                                   	// name
			pack50_description,                      	 	// description
			VIRTUAL_MONEY_PROFILE_1_PACK_50_ID,             // item id
			pack50_quantity,								// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_1_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_1_PACK_50_ID, pack50_cost)
			);
		//profile 2
		public const string VIRTUAL_MONEY_PROFILE_2_PACK_10_ID      = prefix+"virtual_money_p2_pack_10";
		public const string VIRTUAL_MONEY_PROFILE_2_PACK_20_ID      = prefix+"virtual_money_p2_pack_20";
		public const string VIRTUAL_MONEY_PROFILE_2_PACK_50_ID      = prefix+"virtual_money_p2_pack_50";
		
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_2_PACK_10 = new VirtualCurrencyPack(
			pack10_name,                                   	// name
			pack10_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_2_PACK_10_ID,                     // item id
			pack10_quantity,												// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_2_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_2_PACK_10_ID, pack10_cost)
			);
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_2_PACK_20 = new VirtualCurrencyPack(
			pack20_name,                                   	// name
			pack20_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_2_PACK_20_ID,                     // item id
			pack20_quantity,												// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_2_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_2_PACK_20_ID, pack20_cost)
			);
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_2_PACK_50 = new VirtualCurrencyPack(
			pack50_name,                                   	// name
			pack50_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_2_PACK_50_ID,                     // item id
			pack50_quantity,												// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_2_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_2_PACK_50_ID, pack50_cost)
			);
		//profile 3
		public const string VIRTUAL_MONEY_PROFILE_3_PACK_10_ID      = prefix+"virtual_money_p3_pack_10";
		public const string VIRTUAL_MONEY_PROFILE_3_PACK_20_ID      = prefix+"virtual_money_p3_pack_20";
		public const string VIRTUAL_MONEY_PROFILE_3_PACK_50_ID      = prefix+"virtual_money_p3_pack_50";
		
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_3_PACK_10 = new VirtualCurrencyPack(
			pack10_name,                                   	// name
			pack10_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_3_PACK_10_ID,                     // item id
			pack10_quantity,											// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_3_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_3_PACK_10_ID, pack10_cost)
			);
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_3_PACK_20 = new VirtualCurrencyPack(
			pack20_name,                                   	// name
			pack20_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_3_PACK_20_ID,                     // item id
			pack20_quantity,												// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_3_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_3_PACK_20_ID, pack20_cost)
			);
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_3_PACK_50 = new VirtualCurrencyPack(
			pack50_name,                                   	// name
			pack50_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_3_PACK_50_ID,                     // item id
			pack50_quantity,												// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_3_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_3_PACK_50_ID, pack50_cost)
			);
		//profile 4
		public const string VIRTUAL_MONEY_PROFILE_4_PACK_10_ID      = prefix+"virtual_money_p4_pack_10";
		public const string VIRTUAL_MONEY_PROFILE_4_PACK_20_ID      = prefix+"virtual_money_p4_pack_20";
		public const string VIRTUAL_MONEY_PROFILE_4_PACK_50_ID      = prefix+"virtual_money_p4_pack_50";
		
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_4_PACK_10 = new VirtualCurrencyPack(
			pack10_name,                                   	// name
			pack10_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_4_PACK_10_ID,                     // item id
			pack10_quantity,											// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_4_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_4_PACK_10_ID, pack10_cost)
			);
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_4_PACK_20 = new VirtualCurrencyPack(
			pack20_name,                                   	// name
			pack20_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_4_PACK_20_ID,                     // item id
			pack20_quantity,												// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_4_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_4_PACK_20_ID, pack20_cost)
			);
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_4_PACK_50 = new VirtualCurrencyPack(
			pack50_name,                                   	// name
			pack50_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_4_PACK_50_ID,                     // item id
			pack50_quantity,												// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_4_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_4_PACK_50_ID, pack50_cost)
			);
		//profile 5
		public const string VIRTUAL_MONEY_PROFILE_5_PACK_10_ID      = prefix+"virtual_money_p5_pack_10";
		public const string VIRTUAL_MONEY_PROFILE_5_PACK_20_ID      = prefix+"virtual_money_p5_pack_20";
		public const string VIRTUAL_MONEY_PROFILE_5_PACK_50_ID      = prefix+"virtual_money_p5_pack_50";
		
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_5_PACK_10 = new VirtualCurrencyPack(
			pack10_name,                                   	// name
			pack10_description,                     	 		// description
			VIRTUAL_MONEY_PROFILE_5_PACK_10_ID,                     // item id
			pack10_quantity,											// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_5_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_5_PACK_10_ID, pack10_cost)
			);
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_5_PACK_20 = new VirtualCurrencyPack(
			pack20_name,                                   	// name
			pack20_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_5_PACK_20_ID,                     // item id
			pack20_quantity,												// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_5_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_5_PACK_20_ID, pack20_cost)
			);
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_5_PACK_50 = new VirtualCurrencyPack(
			pack50_name,                                   	// name
			pack50_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_5_PACK_50_ID,                     // item id
			pack50_quantity,												// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_5_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_5_PACK_50_ID, pack50_cost)
			);
		//profile 6
		public const string VIRTUAL_MONEY_PROFILE_6_PACK_10_ID      = prefix+"virtual_money_p6_pack_10";
		public const string VIRTUAL_MONEY_PROFILE_6_PACK_20_ID      = prefix+"virtual_money_p6_pack_20";
		public const string VIRTUAL_MONEY_PROFILE_6_PACK_50_ID      = prefix+"virtual_money_p6_pack_50";
		
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_6_PACK_10 = new VirtualCurrencyPack(
			pack10_name,                                   	// name
			pack10_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_6_PACK_10_ID,                     // item id
			pack10_quantity,												// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_6_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_6_PACK_10_ID, pack10_cost)
			);
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_6_PACK_20 = new VirtualCurrencyPack(
			pack20_name,                                   	// name
			pack20_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_6_PACK_20_ID,                     // item id
			pack20_quantity,												// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_6_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_6_PACK_20_ID, pack20_cost)
			);
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_6_PACK_50 = new VirtualCurrencyPack(
			pack50_name,                                   	// name
			pack50_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_6_PACK_50_ID,                     // item id
			pack50_quantity,												// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_6_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_6_PACK_50_ID, pack50_cost)
			);
		//profile 7
		public const string VIRTUAL_MONEY_PROFILE_7_PACK_10_ID      = prefix+"virtual_money_p7_pack_10";
		public const string VIRTUAL_MONEY_PROFILE_7_PACK_20_ID      = prefix+"virtual_money_p7_pack_20";
		public const string VIRTUAL_MONEY_PROFILE_7_PACK_50_ID      = prefix+"virtual_money_p7_pack_50";
		
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_7_PACK_10 = new VirtualCurrencyPack(
			pack10_name,                                   	// name
			pack10_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_7_PACK_10_ID,                     // item id
			pack10_quantity,												// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_7_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_7_PACK_10_ID, pack10_cost)
			);
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_7_PACK_20 = new VirtualCurrencyPack(
			pack20_name,                                   	// name
			pack20_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_7_PACK_20_ID,                     // item id
			pack20_quantity,												// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_7_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_7_PACK_20_ID, pack20_cost)
			);
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_7_PACK_50 = new VirtualCurrencyPack(
			pack50_name,                                   	// name
			pack50_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_7_PACK_50_ID,                     // item id
			pack50_quantity,												// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_7_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_7_PACK_50_ID, pack50_cost)
			);
		//profile 8
		public const string VIRTUAL_MONEY_PROFILE_8_PACK_10_ID      = prefix+"virtual_money_p8_pack_10";
		public const string VIRTUAL_MONEY_PROFILE_8_PACK_20_ID      = prefix+"virtual_money_p8_pack_20";
		public const string VIRTUAL_MONEY_PROFILE_8_PACK_50_ID      = prefix+"virtual_money_p8_pack_50";
		
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_8_PACK_10 = new VirtualCurrencyPack(
			pack10_name,                                 	// name
			pack10_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_8_PACK_10_ID,                     // item id
			pack10_quantity,											// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_8_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_8_PACK_10_ID, pack10_cost)
			);
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_8_PACK_20 = new VirtualCurrencyPack(
			pack20_name,                                   	// name
			pack20_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_8_PACK_20_ID,                     // item id
			pack20_quantity,												// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_8_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_8_PACK_20_ID, pack20_cost)
			);
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_8_PACK_50 = new VirtualCurrencyPack(
			pack50_name,                                   	// name
			pack50_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_8_PACK_50_ID,                     // item id
			pack50_quantity,												// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_8_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_8_PACK_50_ID, pack50_cost)
			);
		//profile 9
		public const string VIRTUAL_MONEY_PROFILE_9_PACK_10_ID      = prefix+"virtual_money_p9_pack_10";
		public const string VIRTUAL_MONEY_PROFILE_9_PACK_20_ID      = prefix+"virtual_money_p9_pack_20";
		public const string VIRTUAL_MONEY_PROFILE_9_PACK_50_ID      = prefix+"virtual_money_p9_pack_50";
		
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_9_PACK_10 = new VirtualCurrencyPack(
			pack10_name,                                 	// name
			pack10_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_9_PACK_10_ID,                     // item id
			pack10_quantity,											// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_9_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_9_PACK_10_ID, pack10_cost)
			);
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_9_PACK_20 = new VirtualCurrencyPack(
			pack20_name,                                   	// name
			pack20_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_9_PACK_20_ID,                     // item id
			pack20_quantity,												// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_9_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_9_PACK_20_ID, pack20_cost)
			);
		public static VirtualCurrencyPack VIRTUAL_MONEY_PROFILE_9_PACK_50 = new VirtualCurrencyPack(
			pack50_name,                                   	// name
			pack50_description,                      	 		// description
			VIRTUAL_MONEY_PROFILE_9_PACK_50_ID,                     // item id
			pack50_quantity,												// number of currencies in the pack
			VIRTUAL_MONEY_PROFILE_9_ID,            			// the currency associated with this pack
			new PurchaseWithMarket(VIRTUAL_MONEY_PROFILE_9_PACK_50_ID, pack50_cost)
			);

		#endregion
		
		//if you don't need this, leave it empty, BUT NOT delete it!
		public VirtualGood[] GetGoods()//consumable items
		{
			return new VirtualGood[]{};//buy one item
		}


		
		//if you don't need this, leave it empty, BUT NOT delete it!
		public VirtualCategory[] GetCategories()
		{
			return new VirtualCategory[]{};
		}
		
		//if you don't need this, leave it empty, BUT NOT delete it!
		public LifetimeVG[] GetNotConsumableItems()//
		{
			return new LifetimeVG[]{};//NO_ADS_NONCONS
		}

		
		public UpgradeVG[] GetIncrementalItems()
		{
			return new UpgradeVG[]{};
		}
		
		
		
	}
}
*/ //DELETE THIS LINE FOR SOOMLA