/******************************************************************************
*                                                                             *
*   PROJECT : EOS Digital Software Development Kit EDSDK                      *
*      NAME : Property.h	                                                  *
*                                                                             *
*   Description: This is the Sample code to show the usage of EDSDK.          *
*                                                                             *
*                                                                             *
*******************************************************************************
*                                                                             *
*   Written and developed by Camera Design Dept.53                            *
*   Copyright Canon Inc. 2006 All Rights Reserved                             *
*                                                                             *
*******************************************************************************
*   File Update Information:                                                  *
*     DATE      Identify    Comment                                           *
*   -----------------------------------------------------------------------   *
*   06-03-22    F-001        create first version.                            *
*                                                                             *
******************************************************************************/


#pragma once 
#include <map>

class Property
{
public:	

	static std::map<EdsUInt32, const char *> getAEModeMap()
	{
		std::map<EdsUInt32, const char *> AEMode;

		AEMode.insert( std::pair<EdsUInt32, const char *>(0,"P"));
		AEMode.insert( std::pair<EdsUInt32, const char *>(1,"Tv"));
		AEMode.insert( std::pair<EdsUInt32, const char *>(2,"Av"));
		AEMode.insert( std::pair<EdsUInt32, const char *>(3,"M"));
		AEMode.insert( std::pair<EdsUInt32, const char *>(4,"Bulb"));
		AEMode.insert( std::pair<EdsUInt32, const char *>(5,"A-DEP"));
		AEMode.insert( std::pair<EdsUInt32, const char *>(6,"DEP"));
		AEMode.insert( std::pair<EdsUInt32, const char *>(7,"C"));
		AEMode.insert( std::pair<EdsUInt32, const char *>(8,"Lock"));
		AEMode.insert( std::pair<EdsUInt32, const char *>(9,"GreenMode"));
		AEMode.insert( std::pair<EdsUInt32, const char *>(10,"Night Portrai"));
		AEMode.insert( std::pair<EdsUInt32, const char *>(11,"Sports"));
		AEMode.insert( std::pair<EdsUInt32, const char *>(12,"Portrait"));
		AEMode.insert( std::pair<EdsUInt32, const char *>(13,"LandScape"));
		AEMode.insert( std::pair<EdsUInt32, const char *>(14,"Close Up"));
		AEMode.insert( std::pair<EdsUInt32, const char *>(15,"No Strobo"));
		AEMode.insert( std::pair<EdsUInt32, const char *>(0xffffffff,"unkown"));

		return AEMode;	
	}


	static std::map<EdsUInt32, const char *> getTvMap()
	{
		std::map<EdsUInt32, const char *> TV;

		TV.insert( std::pair<EdsUInt32, const char *>(0x0c,"Bulb"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x10,"30h"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x13,"25h"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x14,"20h"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x15,"20h"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x18,"15h"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x1B,"13h"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x1C,"10h"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x1D,"10h"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x20,"8h"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x23,"6h"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x24,"6h"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x25,"5h"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x28,"4h"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x2B,"3h2"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x2C,"3h"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x2D,"2h5"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x30,"2h"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x33,"1h6"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x34,"1h5"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x35,"1h3"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x38,"1h"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x3B,"0h8"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x3C,"0h7"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x3D,"0h6"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x40,"0h5"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x43,"0h4"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x44,"0h3"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x45,"0h3"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x48,"4"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x4B,"5"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x4C,"6"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x4D,"6"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x50,"8"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x53,"10"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x54,"10"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x55,"13"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x58,"15"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x5B,"20"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x5C,"20"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x5D,"25"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x60,"30"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x63,"40"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x64,"45"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x65,"50"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x68,"60"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x6B,"80"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x6C,"90"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x6D,"100"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x70,"125"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x73,"160"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x74,"180"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x75,"200"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x78,"250"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x7B,"320"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x7C,"350"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x7D,"400"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x80,"500"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x83,"640"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x84,"750"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x85,"800"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x88,"1000"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x8B,"1250"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x8C,"1500"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x8D,"1600"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x90,"2000"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x93,"2500"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x94,"3000"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x95,"3200"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x98,"4000"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x9B,"5000"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x9C,"6000"));
		TV.insert( std::pair<EdsUInt32, const char *>(0x9D,"6400"));
		TV.insert( std::pair<EdsUInt32, const char *>(0xA0,"8000"));
		TV.insert( std::pair<EdsUInt32, const char *>(0xffffffff,"unkown"));
		
		return TV;
	}


	static std::map<EdsUInt32, const char *> getAvMap()
	{
		std::map<EdsUInt32, const char *> AV;
	
		AV.insert( std::pair<EdsUInt32, const char *>(0x00,"00"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x08,"1"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x0B,"1.1"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x0C,"1.2"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x0D,"1.2"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x10,"1.4"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x13,"1.6"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x14,"1.8"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x15,"1.8"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x18,"2"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x1B,"2.2"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x1C,"2.5"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x1D,"2.5"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x20,"2.8"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x23,"3.2"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x24,"3.5"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x25,"3.5"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x28,"4"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x2B,"4"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x2C,"4.5"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x2D,"5.6"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x30,"5.6"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x33,"6.3"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x34,"6.7"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x35,"7.1"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x38,"8"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x3B,"9"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x3C,"9.5"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x3D,"10"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x40,"11"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x43,"13"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x44,"13"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x45,"14"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x48,"16"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x4B,"18"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x4C,"19"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x4D,"20"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x50,"22"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x53,"25"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x54,"27"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x55,"29"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x58,"32"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x5B,"36"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x5C,"38"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x5D,"40"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x60,"45"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x63,"51"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x64,"54"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x65,"57"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x68,"64"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x6B,"72"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x6C,"76"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x6D,"80"));
		AV.insert( std::pair<EdsUInt32, const char *>(0x70,"91"));
		AV.insert( std::pair<EdsUInt32, const char *>(0xffffffff,"unkown"));

		return AV;
	}

	static std::map<EdsUInt32, const char *> getIsoMap()
	{
		std::map<EdsUInt32, const char *> ISO;

		ISO.insert( std::pair<EdsUInt32, const char *>(0x28,"6"));
		ISO.insert( std::pair<EdsUInt32, const char *>(0x30,"12"));
		ISO.insert( std::pair<EdsUInt32, const char *>(0x38,"25"));
		ISO.insert( std::pair<EdsUInt32, const char *>(0x40,"50"));
		ISO.insert( std::pair<EdsUInt32, const char *>(0x48,"100"));
		ISO.insert( std::pair<EdsUInt32, const char *>(0x4b,"125"));
		ISO.insert( std::pair<EdsUInt32, const char *>(0x4d,"160"));
		ISO.insert( std::pair<EdsUInt32, const char *>(0x50,"200"));
		ISO.insert( std::pair<EdsUInt32, const char *>(0x53,"250"));
		ISO.insert( std::pair<EdsUInt32, const char *>(0x55,"320"));
		ISO.insert( std::pair<EdsUInt32, const char *>(0x58,"400"));
		ISO.insert( std::pair<EdsUInt32, const char *>(0x5b,"500"));
		ISO.insert( std::pair<EdsUInt32, const char *>(0x5d,"640"));
		ISO.insert( std::pair<EdsUInt32, const char *>(0x60,"800"));
		ISO.insert( std::pair<EdsUInt32, const char *>(0x63,"1000"));
		ISO.insert( std::pair<EdsUInt32, const char *>(0x65,"1250"));
		ISO.insert( std::pair<EdsUInt32, const char *>(0x68,"1600"));
		ISO.insert( std::pair<EdsUInt32, const char *>(0x70,"3200"));
		ISO.insert( std::pair<EdsUInt32, const char *>(0x78,"6400"));
		ISO.insert( std::pair<EdsUInt32, const char *>(0xffffffff,"unkown"));
	
		return ISO;
	}

	static std::map<EdsUInt32, const char *> getMeteringModeMap()
	{
		std::map<EdsUInt32, const char *> MeteringMode;

		MeteringMode.insert( std::pair<EdsUInt32, const char *>(1,"Spot Metering"));
		MeteringMode.insert( std::pair<EdsUInt32, const char *>(3,"Evaluative Metering"));
		MeteringMode.insert( std::pair<EdsUInt32, const char *>(4,"Partial Metering"));
		MeteringMode.insert( std::pair<EdsUInt32, const char *>(5,"Center-Weighted Average Metering"));
		MeteringMode.insert( std::pair<EdsUInt32, const char *>(0xffffffff,"unkown"));
	
		return MeteringMode;
	}

	static std::map<EdsUInt32, const char *> getExposureCompMap()
	{
		std::map<EdsUInt32, const char *> ExposureComp;

		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0x18,"+3"));
		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0x15,"+2 2/3"));
		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0x14,"+2 1/2"));
		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0x13,"+2 1/3"));
		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0x10,"+2"));
		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0x0d,"+1 2/3"));
		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0x0c,"+1 1/2"));
		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0x0b,"+1 1/3"));
		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0x08,"+1"));
		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0x05,"+2/3"));
		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0x04,"+1/2"));
		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0x03,"+1/3"));
		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0x00,"0"));
		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0xfd,"-1/3"));
		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0xfc,"-1/2"));
		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0xfb,"-2/3"));
		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0xf8,"-1"));
		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0xf5,"-1 1/3"));
		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0xf4,"-1 1/2"));
		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0xf3,"-1 2/3"));
		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0xf0,"-2"));
		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0xed,"-2 1/3"));
		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0xec,"-2 1/2"));
		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0xeb,"-2 2/3"));
		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0xe8,"-3"));
		ExposureComp.insert( std::pair<EdsUInt32, const char *>(0xffffffff,"unkown"));
	
		return ExposureComp;
	}


};