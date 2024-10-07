/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Techne.Plugins.Interfaces;

namespace Techne
{
    public class DeobfuscatedImporter
    {
        private readonly string[] javaLines;
        private string className;

        public DeobfuscatedImporter(string path)
        {
            //javaLines = File.ReadAllLines(path);

            //            javaLines = @"public class Dragon
            //public Dragon
            //{
            //wing2part2 = new ModelRenderer(44, 48);
            //wingroot2 = new ModelRenderer(18, 108);
            //foot3 = new ModelRenderer(0, 64);
            //tail2 = new ModelRenderer(0, 95);
            //head = new ModelRenderer(85, 7);
            //tail3 = new ModelRenderer(15, 41);
            //nail3foot2 = new ModelRenderer(7, 83);
            //box0 = new ModelRenderer(7, 89);
            //tail1 = new ModelRenderer(18, 48);
            //nail2foot4 = new ModelRenderer(6, 92);
            //wingroot1 = new ModelRenderer(14, 29);
            //nail2foot5 = new ModelRenderer(0, 86);
            //nail1foot4 = new ModelRenderer(0, 89);
            //nail1 = new ModelRenderer(7, 77);
            //wing2part3 = new ModelRenderer(0, 80);
            //foot1 = new ModelRenderer(0, 22);
            //nail2foot3 = new ModelRenderer(7, 80);
            //wing1part1 = new ModelRenderer(58, 114);
            //wing1part2 = new ModelRenderer(52, 97);
            //tail4 = new ModelRenderer(0, 73);
            //leg3 = new ModelRenderer(0, 0);
            //nail2foot2 = new ModelRenderer(0, 92);
            //leg1 = new ModelRenderer(0, 29);
            //nail1foot3 = new ModelRenderer(0, 80);
            //upperleg4 = new ModelRenderer(22, 64);
            //foot4 = new ModelRenderer(0, 105);
            //box = new ModelRenderer(0, 83);
            //leg4 = new ModelRenderer(0, 0);
            //leg2 = new ModelRenderer(0, 43);
            //nail3foot3 = new ModelRenderer(0, 77);
            //wing1part4 = new ModelRenderer(0, 114);
            //uppperleg1 = new ModelRenderer(40, 64);
            //foot2 = new ModelRenderer(0, 57);
            //upperleg2 = new ModelRenderer(108, 85);
            //body3 = new ModelRenderer(30, 95);
            //neck = new ModelRenderer(0, 14);
            //nail1foot2 = new ModelRenderer(7, 86);
            //wing1part3 = new ModelRenderer(28, 24);
            //body2 = new ModelRenderer(26, 0);
            //upperleg3 = new ModelRenderer(45, 112);
            //body1 = new ModelRenderer(74, 64);
            //
            //wing2part2.addBox(0F, 0F, 0F, 20, 1, 14);
            //wing2part2.setPosition(0F, 27F, -31F);
            //wingroot2.addBox(0F, 0F, 0F, 3, 1, 4);
            //wingroot2.setPosition(0F, 27F, -4F);
            //foot3.addBox(0F, 0F, 0F, 6, 2, 5);
            //foot3.setPosition(19F, 0F, 7F);
            //tail2.addBox(0F, 0F, 0F, 10, 5, 5);
            //tail2.setPosition(39F, 23F, -1F);
            //head.addBox(0F, 0F, 0F, 10, 9, 7);
            //head.setPosition(-20F, 32F, 3F);
            //tail3.addBox(0F, 0F, 0F, 7, 3, 3);
            //tail3.setPosition(48F, 24F, -4F);
            //nail3foot2.addBox(0F, 0F, 0F, 2, 1, 1);
            //nail3foot2.setPosition(0F, 0F, 7F);
            //box0.addBox(0F, 0F, -3F, 2, 1, 1);
            //box0.setPosition(0F, 0F, 2F);
            //tail1.addBox(0F, 0F, 0F, 13, 7, 7);
            //tail1.setPosition(27F, 22F, 1F);
            //nail2foot4.addBox(0F, 0F, 0F, 2, 1, 1);
            //nail2foot4.setPosition(17F, 0F, -1F);
            //wingroot1.addBox(0F, 0F, 0F, 3, 1, 4);
            //wingroot1.setPosition(0F, 27F, 12F);
            //nail2foot5.addBox(0F, 0F, 0F, 2, 1, 1);
            //nail2foot5.setPosition(17F, 0F, 1F);
            //nail1foot4.addBox(0F, 0F, 0F, 2, 1, 1);
            //nail1foot4.setPosition(17F, 0F, -3F);
            //nail1.addBox(0F, 0F, 0F, 2, 1, 1);
            //nail1.setPosition(0F, 0F, 3F);
            //wing2part3.addBox(0F, 0F, 0F, 30, 1, 14);
            //wing2part3.setPosition(0F, 27F, -45F);
            //foot1.addBox(0F, 0F, 0F, 6, 2, 5);
            //foot1.setPosition(2F, 0F, -1F);
            //nail2foot3.addBox(0F, 0F, 0F, 2, 1, 1);
            //nail2foot3.setPosition(17F, 0F, 9F);
            //wing1part1.addBox(0F, 0F, 0F, 16, 1, 13);
            //wing1part1.setPosition(0F, 27F, 16F);
            //wing1part2.addBox(0F, 0F, 0F, 20, 1, 14);
            //wing1part2.setPosition(0F, 27F, 29F);
            //tail4.addBox(0F, 0F, 0F, 7, 1, 1);
            //tail4.setPosition(55F, 25F, -6F);
            //leg3.addBox(0F, 0F, 0F, 4, 9, 5);
            //leg3.setPosition(20F, 2F, 7F);
            //nail2foot2.addBox(0F, 0F, 0F, 2, 1, 1);
            //nail2foot2.setPosition(0F, 0F, 9F);
            //leg1.addBox(-3F, -1F, -3F, 4, 9, 5);
            //leg1.setPosition(6F, 3F, 2F);
            //nail1foot3.addBox(0F, 0F, 0F, 2, 1, 1);
            //nail1foot3.setPosition(17F, 0F, 7F);
            //upperleg4.addBox(0F, 0F, 0F, 4, 11, 5);
            //upperleg4.setPosition(22F, 8F, -2F);
            //foot4.addBox(0F, 0F, 0F, 6, 2, 5);
            //foot4.setPosition(19F, 0F, -3F);
            //box.addBox(0F, 0F, 0F, 2, 1, 1);
            //box.setPosition(0F, 0F, 1F);
            //leg4.addBox(-3F, -1F, -3F, 4, 9, 5);
            //leg4.setPosition(23F, 2F, 0F);
            //leg2.addBox(0F, 0F, 0F, 4, 9, 5);
            //leg2.setPosition(3F, 2F, 7F);
            //nail3foot3.addBox(0F, 0F, 0F, 2, 1, 1);
            //nail3foot3.setPosition(17F, 0F, 11F);
            //wing1part4.addBox(0F, 0F, 0F, 16, 1, 13);
            //wing1part4.setPosition(0F, 27F, -17F);
            //uppperleg1.addBox(0F, 0F, 0F, 4, 11, 5);
            //uppperleg1.setPosition(5F, 9F, -1F);
            //foot2.addBox(0F, 0F, 0F, 6, 2, 5);
            //foot2.setPosition(2F, 0F, 7F);
            //upperleg2.addBox(0F, 0F, 0F, 4, 11, 5);
            //upperleg2.setPosition(5F, 9F, 7F);
            //body3.addBox(0F, 0F, 0F, 10, 8, 8);
            //body3.setPosition(-7F, 27F, 2F);
            //neck.addBox(0F, 0F, 0F, 9, 4, 4);
            //neck.setPosition(-13F, 33F, 4F);
            //nail1foot2.addBox(0F, 0F, 0F, 2, 1, 1);
            //nail1foot2.setPosition(0F, 0F, 11F);
            //wing1part3.addBox(0F, 0F, 0F, 30, 1, 14);
            //wing1part3.setPosition(0F, 27F, 43F);
            //body2.addBox(0F, 5F, 0F, 17, 11, 12);
            //body2.setPosition(0F, 16F, 0F);
            //upperleg3.addBox(0F, 0F, 0F, 4, 10, 5);
            //upperleg3.setPosition(22F, 8F, 7F);
            //body1.addBox(1F, -3F, 0F, 15, 9, 10);
            //body1.setPosition(15F, 19F, 0F);
            //
            //wing2part2.rotateAngleZ = 0.22689280275926285F;
            //wingroot2.rotateAngleZ = 0.22689280275926285F;
            //tail2.rotateAngleY = 5.934119456780721F;
            //tail3.rotateAngleY = 5.8643062867009474F;
            //tail1.rotateAngleY = 6.056292504420323F;
            //wingroot1.rotateAngleZ = 0.22689280275926285F;
            //nail1.rotateAngleY = 6.283185307179586F;
            //wing2part3.rotateAngleZ = 0.22689280275926285F;
            //foot1.rotateAngleY = 6.283185307179586F;
            //wing1part1.rotateAngleZ = 0.22689280275926285F;
            //wing1part2.rotateAngleZ = 0.22689280275926285F;
            //tail4.rotateAngleY = 5.794493116621174F;
            //leg3.rotateAngleX = 0.03490658503988659F;
            //leg3.rotateAngleZ = 0.22689280275926285F;
            //leg1.rotateAngleX = 6.283185307179586F;
            //leg1.rotateAngleY = 6.283185307179586F;
            //leg1.rotateAngleZ = 0.22689280275926285F;
            //upperleg4.rotateAngleX = 6.073745796940266F;
            //upperleg4.rotateAngleZ = 5.98647933434055F;
            //foot4.rotateAngleY = 6.283185307179586F;
            //box.rotateAngleY = 6.283185307179586F;
            //leg4.rotateAngleX = 6.09119908946021F;
            //leg4.rotateAngleZ = 0.15707963267948966F;
            //leg2.rotateAngleZ = 0.22689280275926285F;
            //wing1part4.rotateAngleZ = 0.22689280275926285F;
            //uppperleg1.rotateAngleX = 6.19591884457987F;
            //uppperleg1.rotateAngleY = 6.283185307179586F;
            //uppperleg1.rotateAngleZ = 6.19591884457987F;
            //upperleg2.rotateAngleZ = 6.19591884457987F;
            //body3.rotateAngleZ = 0.4886921905584123F;
            //neck.rotateAngleZ = 0.47123889803846897F;
            //wing1part3.rotateAngleZ = 0.24434609527920614F;
            //body2.rotateAngleZ = 0.22689280275926285F;
            //upperleg3.rotateAngleX = 0.20943951023931956F;
            //upperleg3.rotateAngleZ = 5.98647933434055F;
            //body1.rotateAngleY = 6.283185307179586F;
            //body1.rotateAngleZ = 5.969026041820607F;
            //}".Replace("\r\n", "\n").Split('\n');

            //            javaLines = @"package net.minecraft.src;
            //
            //public class ModelDragon extends ModelBase
            //
            //{
            //
            //public ModelDragon()
            //{
            //BackLeftFoot = new ModelRenderer(0, 0);
            //BackLeftFoot.addBox(0F, 0F, 0F, 16, 16, 16);
            //BackLeftFoot.setPosition(-48F, -6F, 32F);
            //BackRightFoot = new ModelRenderer(0, 0);
            //BackRightFoot.addBox(0F, 0F, 0F, 16, 16, 16);
            //BackRightFoot.setPosition(-48F, -6F, 0F);
            //Belly = new ModelRenderer(0, 0);
            //Belly.addBox(0F, 0F, 0F, 64, 16, 48);
            //Belly.setPosition(-48F, 10F, 0F);
            //FrontLeftFoot = new ModelRenderer(0, 0);
            //FrontLeftFoot.addBox(0F, 0F, 0F, 16, 16, 16);
            //FrontLeftFoot.setPosition(0F, -6F, 32F);
            //FrontRightFoot = new ModelRenderer(0, 0);
            //FrontRightFoot.addBox(0F, 0F, 0F, 16, 16, 16);
            //FrontRightFoot.setPosition(0F, -6F, 0F);
            //Head = new ModelRenderer(0, 0);
            //Head.addBox(0F, 0F, 0F, 32, 16, 48);
            //Head.setPosition(0F, 42F, 0F);
            //LeftWing = new ModelRenderer(0, 0);
            //LeftWing.addBox(0F, 0F, 0F, 16, 16, 16);
            //LeftWing.setPosition(-16F, 10F, 48F);
            //Neck = new ModelRenderer(0, 0);
            //Neck.addBox(0F, 0F, 0F, 16, 16, 48);
            //Neck.setPosition(0F, 26F, 0F);
            //RightWing = new ModelRenderer(0, 0);
            //RightWing.addBox(0F, 0F, 0F, 16, 16, 16);
            //RightWing.setPosition(-16F, 10F, -16F);
            //Tail = new ModelRenderer(0, 0);
            //Tail.addBox(0F, 0F, 0F, 32, 16, 16);
            //Tail.setPosition(-80F, 10F, 16F);
            //}
            //
            //
            //public void render(float f, float f1, float f2, float f3, float f4, float f5)
            //
            //{
            //
            ////render:
            //setRotationAngles(f, f1, f2, f3, f4, f5);
            //BackLeftFoot.render(f5);
            //BackRightFoot.render(f5);
            //Belly.render(f5);
            //FrontLeftFoot.render(f5);
            //FrontRightFoot.render(f5);
            //Head.render(f5);
            //LeftWing.render(f5);
            //Neck.render(f5);
            //RightWing.render(f5);
            //Tail.render(f5);
            //
            //}
            //
            //
            //
            ////variables init:
            //public ModelRenderer BackLeftFoot;
            //public ModelRenderer BackRightFoot;
            //public ModelRenderer Belly;
            //public ModelRenderer FrontLeftFoot;
            //public ModelRenderer FrontRightFoot;
            //public ModelRenderer Head;
            //public ModelRenderer LeftWing;
            //public ModelRenderer Neck;
            //public ModelRenderer RightWing;
            //public ModelRenderer Tail;
            //
            //}".Replace("\r\n", "\n").Split('\n');

            //            javaLines = @"package net.minecraft.src;
            ////Exported java file
            ////Keep in mind that you still need to fill in some blanks
            //// - ZeuX
            //
            //public class Model extends ModelBase
            //{
            //
            //public Model()
            //{
            //	float scale = 0F;
            //	head = new ModelRenderer(0, 0);
            //	head.addBox(-3F, 3F, -3F, 6, 6, 4, scale);
            //	head.setPosition(0F, 0F, 0F);
            //
            //	body = new ModelRenderer(16, 16);
            //	body.addBox(-3F, 9F, -2F, 6, 9, 2, scale);
            //	body.setPosition(0F, 0F, 0F);
            //
            //	rightarm = new ModelRenderer(40, 16);
            //	rightarm.addBox(-3F, -2F, -2F, 2, 8, 2, scale);
            //	rightarm.setPosition(-2F, 11F, 0F);
            //
            //	leftarm = new ModelRenderer(40, 16);
            //	leftarm.addBox(-2F, 7F, -2F, 2, 8, 2, scale);
            //	leftarm.setPosition(5F, 2F, 0F);
            //
            //	rightleg = new ModelRenderer(0, 16);
            //	rightleg.addBox(0F, 5F, -2F, 2, 6, 2, scale);
            //	rightleg.setPosition(-2F, 13F, 0F);
            //
            //	leftleg = new ModelRenderer(0, 16);
            //	leftleg.addBox(-2F, 2F, -2F, 2, 6, 2, scale);
            //	leftleg.setPosition(2F, 16F, 0F);
            //
            //}
            //public void render(float f, float f1, float f2, float f3, float f4, float f5)
            //{
            ////for animation
            ////setRotationAngles(f, f1, f2, f3, f4, f5);
            //	head.render(f5);
            //	body.render(f5);
            //	rightarm.render(f5);
            //	leftarm.render(f5);
            //	rightleg.render(f5);
            //	leftleg.render(f5);
            //}
            //
            ////fields
            //
            //	ModelRenderer head;
            //	ModelRenderer body;
            //	ModelRenderer rightarm;
            //	ModelRenderer leftarm;
            //	ModelRenderer rightleg;
            //	ModelRenderer leftleg;
            //
            //}".Replace("\r\n", "\n").Split('\n');

            //            javaLines = @"
            //package net.minecraft.src;
            //
            //public class Family_iPixelisFemaleModel extends ModelBase
            //{
            //
            //	public ModelRenderer a;
            //	public ModelRenderer b;
            //	public ModelRenderer c;
            //	public ModelRenderer d;
            //	public ModelRenderer e;
            //	public ModelRenderer n;
            //	public ModelRenderer g;
            //	public ModelRenderer h;
            //	public ModelRenderer i;
            //	public ModelRenderer bipedChestT1;
            //	public ModelRenderer bipedChestT2;
            //	public ModelRenderer bipedChestF1;
            //	public ModelRenderer bipedChestF2;
            //	public ModelRenderer bipedChestF3;
            //	public ModelRenderer bipedChestF4;
            //	
            //	public Family_iPixelisFemaleModel()
            //	{
            //		float 0.0F = 0F;
            //		
            //		i = new ModelRenderer(0, 0);
            //	    i.addBox(-5.0F, 0.0F, -1.0F, 10, 16, 1, 0.0F);
            //	    
            //	    h = new ModelRenderer(24, 0);
            //	    h.addBox(-3.0F, -6.0F, -1.0F, 6, 6, 1, 0.0F);
            //	    
            //	    a = new ModelRenderer(0, 0);
            //	    a.addBox(-4.0F, -8.0F, -4.0F, 8, 8, 8, 0.0F);
            //	    a.setPosition(0.0F, 0.0F, 0.0F);
            //	    
            //	    b = new ModelRenderer(32, 0);
            //	    b.addBox(-4.0F, -8.0F, -4.0F, 8, 8, 8, 0.5F);
            //	    b.setPosition(0.0F, 0.0F, 0.0F);
            //	    
            //	    c = new ModelRenderer(16, 16);
            //	    c.addBox(-4.0F, 0.0F, -2.0F, 8, 12, 4, 0.0F);
            //	    c.setPosition(0.0F, 0.0F, 0.0F);
            //	    
            //	    bipedChestT1 = new ModelRenderer(20, 20);
            //	    bipedChestT1.addBox(-3.0F, 1.0F, -3.0F, 6, 1, 1);
            //	    bipedChestT1.setPosition(0.0F, 0.0F, 0.0F);
            //	    
            //	    bipedChestT2 = new ModelRenderer(19, 21);
            //	    bipedChestT2.addBox(-4.0F, 2.0F, -3.0F, 8, 3, 1);
            //	    bipedChestT2.setPosition(0.0F, 0.0F, 0.0F);
            //	    
            //	    bipedChestF1 = new ModelRenderer(20, 21);
            //	    bipedChestF1.addBox(-3.0F, 2.0F, -4.0F, 6, 1, 1);
            //	    bipedChestF1.setPosition(0.0F, 0.0F, 0.0F);
            //	    
            //	    bipedChestF2 = new ModelRenderer(19, 22);
            //	    bipedChestF2.addBox(-4.0F, 3.0F, -4.0F, 8, 1, 1);
            //	    bipedChestF2.setPosition(0.0F, 0.0F, 0.0F);
            //	    
            //	    bipedChestF3 = new ModelRenderer(20, 23);
            //	    bipedChestF3.addBox(-3.0F, 4.0F, -4.0F, 2, 1, 1);
            //	    bipedChestF3.setPosition(0.0F, 0.0F, 0.0F);
            //	    
            //	    bipedChestF4 = new ModelRenderer(23, 23);
            //	    bipedChestF4.addBox(1.0F, 4.0F, -4.0F, 2, 1, 1);
            //	    bipedChestF4.setPosition(0.0F, 0.0F, 0.0F);
            //	    
            //	    d = new ModelRenderer(40, 16);
            //	    d.addBox(-3.0F, -2.0F, -2.0F, 4, 12, 4, 0.0F);
            //	    d.setPosition(-5.0F, 2.0F, 0.0F);
            //	    
            //	    e = new ModelRenderer(40, 16);
            //	    e.mirror = true;
            //	    e.addBox(-1.0F, -2.0F, -2.0F, 4, 12, 4, 0.0F);
            //	    e.setPosition(5.0F, 2.0F, 0.0F);
            //	    
            //	    n = new ModelRenderer(0, 16);
            //	    n.addBox(-2.0F, 0.0F, -2.0F, 4, 12, 4, 0.0F);
            //	    n.setPosition(-2.0F, 12.0F, 0.0F);
            //	    
            //	    g = new ModelRenderer(0, 16);
            //	    g.mirror = true;
            //	    g.addBox(-2.0F, 0.0F, -2.0F, 4, 12, 4, 0.0F);
            //	    g.setPosition(2.0F, 12.0F, 0.0F);
            //	}
            //	
            //	public void render(float f, float f1, float f2, float f3, float f4, float f5)
            //	{
            //	    setRotationAngles(f, f1, f2, f3, f4, f5);
            //	    a.render(f5);
            //	    b.render(f5);
            //	    c.render(f5);
            //	    d.render(f5);
            //	    e.render(f5);
            //	    n.render(f5);
            //	    g.render(f5);
            //	    bipedChestT1.render(f5);
            //	    bipedChestT2.render(f5);
            //	    bipedChestF1.render(f5);
            //	    bipedChestF2.render(f5);
            //	    bipedChestF3.render(f5);
            //	    bipedChestF4.render(f5);
            //	}
            //}
            //".Replace("\r\n", "\n").Split('\n');

            javaLines =
                @"//variables init:
public ModelRenderer antenna1;
public ModelRenderer antenna2;
public ModelRenderer body;
public ModelRenderer leg1;
public ModelRenderer leg2;
public ModelRenderer leg3;
public ModelRenderer leg4;
public ModelRenderer leg5;
public ModelRenderer leg6;

public class ModelDragon extends ModelBase

{

public ModelDragon()
{
antenna1 = new ModelRenderer(0, 0);
antenna1.addBox(-1F, -2F, -1F, 2, 6, 2);
antenna1.setPosition(3F, 6F, 0F);
antenna1.rotateAngleX = 1.117010721276371F;
antenna1.rotateAngleY = 5.951572749300664F;

antenna2 = new ModelRenderer(0, 0);
antenna2.addBox(5F, -1F, 1F, 2, 6, 2);
antenna2.setPosition(3F, 4F, -2F);
antenna2.rotateAngleX = 1.1519173063162575F;
antenna2.rotateAngleY = 0.33161255787892263F;
antenna2.rotateAngleZ = 0.03490658503988659F;

body = new ModelRenderer(0, 0);
body.addBox(0F, 0F, 0F, 12, 6, 16);

leg1 = new ModelRenderer(0, 0);
leg1.addBox(0F, -2F, 0F, 2, 8, 2);
leg1.setPosition(1F, 0F, 13F);
leg1.rotateAngleX = 2.076941809873252F;
leg1.rotateAngleY = 4.310963252425994F;

leg2 = new ModelRenderer(0, 0);
leg2.addBox(7F, -5F, -1F, 2, 8, 2);
leg2.setPosition(-2F, 0F, 16F);
leg2.rotateAngleX = 2.076941809873252F;
leg2.rotateAngleY = 4.590215932745087F;

leg3 = new ModelRenderer(0, 0);
leg3.addBox(2F, -6F, -2F, 2, 8, 2);
leg3.setPosition(-5F, 0F, 5F);
leg3.rotateAngleX = 2.076941809873252F;
leg3.rotateAngleY = 5.009094953223726F;

leg4 = new ModelRenderer(0, 0);
leg4.addBox(0F, -2F, 0F, 2, 8, 2);
leg4.setPosition(12F, 0F, 11F);
leg4.rotateAngleX = 2.076941809873252F;
leg4.rotateAngleY = 1.9722220547535925F;

leg5 = new ModelRenderer(0, 0);
leg5.addBox(7F, -4F, -1F, 2, 8, 2);
leg5.setPosition(15F, 0F, 0F);
leg5.rotateAngleX = 2.076941809873252F;
leg5.rotateAngleY = 1.6929693744344996F;

leg6 = new ModelRenderer(0, 0);
leg6.addBox(2F, -6F, -2F, 2, 8, 2);
leg6.setPosition(15F, 0F, 0F);
leg6.rotateAngleX = 2.076941809873252F;
leg6.rotateAngleY = 1.2740903539558606F;
}

//render:
antenna1.render(f5);
antenna2.render(f5);
body.render(f5);
leg1.render(f5);
leg2.render(f5);
leg3.render(f5);
leg4.render(f5);
leg5.render(f5);
leg6.render(f5);
}"
                    .Replace("\r\n", "\n").Split('\n');
            ;

            for (int i = 0; i < javaLines.Length; i++)
            {
                javaLines[i] = javaLines[i].TrimStart(new[] {' ', '\t'});
                javaLines[i] = javaLines[i].TrimEnd(new[] {' ', '\t'});
            }
        }

        internal Dictionary<string, ITechneVisual> Parse(Dictionary<string, IShapePlugin> shapes)
        {
            ScaleTransform stTemp = new ScaleTransform();


            Dictionary<string, ITechneVisual> cubes = new Dictionary<string, ITechneVisual>();

            className = GetClassName();
            int pCount = 0;
            bool parsing = true;
            int i = 0;

            foreach (var line in javaLines)
            {
                var l = line.TrimStart(new[] {' ', '\t'});

                if (l.StartsWith("public " + className))
                {
                    break;
                }
                i++;
            }

            i++;

            string currentRenderer = "";
            double originX = 0;
            double originY = 0;
            double originZ = 0;
            double width = 0;
            double height = 0;
            double depth = 0;
            double scale = 0;
            int txX = 0;
            int txY = 0;

            for (; i < javaLines.Length && parsing; i++)
            {
                if (javaLines[i].StartsWith("{") || javaLines[i].EndsWith("{"))
                    pCount++;

                if (javaLines[i].StartsWith("}"))
                    pCount--;

                if (pCount <= 0)
                {
                    parsing = false;
                }

                if (javaLines[i].Contains("= new ModelRenderer"))
                {
                    currentRenderer = javaLines[i].Substring(0, javaLines[i].IndexOf('=')).Trim();
                    string line = javaLines[i].Substring(javaLines[i].IndexOf('('));
                    var parts = line.Split(',');

                    txX = Int32.Parse(parts[0].Trim(new[] {'F', ' ', ';', '(', ')'}));
                    txY = Int32.Parse(parts[1].Trim(new[] {'F', ' ', ';', '(', ')'}));

                    var shape = shapes["D9E621F7-957F-4B77-B1AE-20DCD0DA7751".ToLower()].CreateVisual();

                    cubes.Add(currentRenderer, shape);
                    cubes[currentRenderer].TextureOffset = new Vector(txX, txY);
                    cubes[currentRenderer].Name = currentRenderer;
                }
                else if (javaLines[i].Contains("addBox"))
                {
                    currentRenderer = javaLines[i].Substring(0, javaLines[i].IndexOf('.')).Trim();

                    string line = javaLines[i].Substring(javaLines[i].IndexOf('('));

                    var parts = line.Split(',');

                    originX = double.Parse(parts[0].Trim(new[] {'F', ' ', ';', '(', ')'}));
                    originY = double.Parse(parts[1].Trim(new[] {'F', ' ', ';', '(', ')'}));
                    originZ = double.Parse(parts[2].Trim(new[] {'F', ' ', ';', '(', ')'}));

                    height = double.Parse(parts[3].Trim(new[] {'F', ' ', ';', '(', ')'}));
                    width = double.Parse(parts[4].Trim(new[] {'F', ' ', ';', '(', ')'}));
                    depth = double.Parse(parts[5].Trim(new[] {'F', ' ', ';', '(', ')'}));

                    //scale = double.Parse(parts[6].Trim(new char[] { 'F', ' ', ';', '(', ')' }));

                    //Trace.Write(width);
                    //Trace.Write(" ");
                    //Trace.Write(height);
                    //Trace.Write(" ");
                    //Trace.WriteLine(depth);

                    cubes[currentRenderer].Width = height;
                    cubes[currentRenderer].Height = depth;
                    cubes[currentRenderer].Length = width;

                    cubes[currentRenderer].Offset = new Vector3D(originX, originY, originZ);

                    //int viewBoxX = (int)(height + depth) * 2;
                    //int viewBoxY = (int)(width + depth);

                    //brush.Viewbox = new System.Windows.Rect((double)txX, (double)txY, viewBoxX, viewBoxY);
                    //brush.Stretch = Stretch.Fill;
                }
                else if (javaLines[i].Contains("setPosition"))
                {
                    currentRenderer = javaLines[i].Substring(0, javaLines[i].IndexOf('.')).Trim();

                    double x = 0;
                    double y = 0;
                    double z = 0;

                    string line = javaLines[i].Substring(javaLines[i].IndexOf('('));

                    var parts = line.Split(',');

                    var part0 = parts[0].Trim(new[] {'F', ' ', ';', '(', ')'});
                    x = double.Parse(part0, CultureInfo.InvariantCulture);
                    y = double.Parse(parts[1].Trim(new[] {'F', ' ', ';', '(', ')'}), CultureInfo.InvariantCulture);
                    z = double.Parse(parts[2].Trim(new[] {'F', ' ', ';', '(', ')'}), CultureInfo.InvariantCulture);

                    cubes[currentRenderer].Position = new Vector3D(x, y, z);

                    //cubes[currentRenderer].Fill = new SolidColorBrush(Color.FromRgb(125, 125, 0));
                    //cubes[currentRenderer].Material = new DiffuseMaterial(brush);
                }
                //else if (javaLines[i].Contains("rotateAngleX"))
                //{
                //    currentRenderer = javaLines[i].Substring(0, javaLines[i].IndexOf('.')).Trim();
                //    double angle = double.Parse(javaLines[i].Substring(javaLines[i].IndexOf('=') + 2).Trim(new char[] { 'F', ' ', ';', '(', ')' }), CultureInfo.InvariantCulture);
                //    cubes[currentRenderer].RotationX = angle * 180 / Math.PI;
                //}
                //else if (javaLines[i].Contains("rotateAngleY"))
                //{
                //    currentRenderer = javaLines[i].Substring(0, javaLines[i].IndexOf('.')).Trim();
                //    double angle = double.Parse(javaLines[i].Substring(javaLines[i].IndexOf('=') + 2).Trim(new char[] { 'F', ' ', ';', '(', ')' }), CultureInfo.InvariantCulture);
                //    cubes[currentRenderer].RotationY = angle * 180 / Math.PI;
                //}
                //else if (javaLines[i].Contains("rotateAngleZ"))
                //{
                //    currentRenderer = javaLines[i].Substring(0, javaLines[i].IndexOf('.')).Trim();
                //    double angle = double.Parse(javaLines[i].Substring(javaLines[i].IndexOf('=') + 2).Trim(new char[] { 'F', ' ', ';', '(', ')' }), CultureInfo.InvariantCulture);
                //    cubes[currentRenderer].RotationZ = angle * 180 / Math.PI;
                //}
            }

            return cubes;
        }

        private string GetClassName()
        {
            foreach (var line in javaLines)
            {
                if (line.StartsWith("public class "))
                {
                    string result = line.Substring(13).Trim();

                    int pos = result.IndexOf(' ');
                    if (pos >= 0)
                        result = result.Substring(0, pos);

                    return result;
                }
            }

            throw new ArgumentException();
        }

        private Point3D Add(Point3D point3D, double x, double y, double z)
        {
            point3D.X += x;
            point3D.Y += y;
            point3D.Y += y;

            return point3D;
        }

        public Point3D Add(Point3D p1, Point3D p2)
        {
            return new Point3D(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }
    }
}

