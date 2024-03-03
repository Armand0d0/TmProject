package engine;

import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.xml.sax.SAXException;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import java.io.IOException;

public class XMLDecoder {

	public XMLDecoder() {
	}
public String decode(String fileName,String path,String[] def,String attribute) {
	

	DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
	String s = "";
	try {
	DocumentBuilder builder = factory.newDocumentBuilder();
	Document doc = builder.parse("./rescources/models/" + fileName);
	
	
	
	NodeList nodeList = doc.getElementsByTagName(path);
	Node[] nodes = toNodeArr(nodeList);

	//for(int i=0;i<path.length;i++) {
		
		int l = def.length;
		
		
		for(Node n : nodes) {
			
			if(n!=null && n.getNodeType()==Node.ELEMENT_NODE) {
				Element e = (Element)n;
				String DEF = e.getAttribute("DEF");
				//System.out.println(DEF);
				int j = 0;
					
				if(DEF.equals(def[j]) ) {
					if(l==1) { s = e.getAttribute(attribute);}
					NodeList nL = n.getChildNodes();
					nodes = toNodeArr(nL);
					System.out.println(n.getNodeName() + " 0"); 
					
					for(Node n1 : nodes) {
						j=1;
						if(n1!=null && n1.getNodeType()==Node.ELEMENT_NODE) {
							Element e1 = (Element)n1;
							String DEF1 = e1.getAttribute("DEF");
							if(DEF1.equals(def[j])) {
								if(l==2) { s = e1.getAttribute(attribute);}
						System.out.println(n1.getNodeName()+" 1");
						NodeList nL1 = n1.getChildNodes();
						nodes = toNodeArr(nL1);
						
						for(Node n2 : nodes) {
							j=2;
							if(n2!=null && n2.getNodeType()==Node.ELEMENT_NODE) {
								Element e2 = (Element)n2;
								String DEF2 = e2.getAttribute("DEF");
								if(DEF2.equals(def[j])) {
									if(l==3) { s = e2.getAttribute(attribute);}
								System.out.println(n2.getNodeName()+" 2");
								NodeList nL2 = n2.getChildNodes();
								nodes = toNodeArr(nL2);
								
								for(Node n3 : nodes) {
									j=3;
									if(n3!=null && n3.getNodeType()==Node.ELEMENT_NODE) {
										Element e3 = (Element)n3;
										String DEF3 = e3.getAttribute("DEF");
										if(DEF3.equals(def[j])) {
											if(l==4) { s = e3.getAttribute(attribute);}
										System.out.println(n3.getNodeName()+" 3");
										NodeList nL3 = n3.getChildNodes();
										nodes = toNodeArr(nL3);
										
										for(Node n4 : nodes) {
											j=4;
											if(n4!=null && n4.getNodeType()==Node.ELEMENT_NODE) {
												Element e4 = (Element)n4;
												String DEF4 = e4.getAttribute("DEF");
												if(l>4) {
												if(DEF4.equals(def[j])) {
													if(l==5) { s = e4.getAttribute(attribute);}
												System.out.println(n4.getNodeName()+" 4");
												NodeList nL4 = n4.getChildNodes();
												nodes = toNodeArr(nL4);
												
												for(Node n5 : nodes) {
													j=5;
													if(n5!=null && n5.getNodeType()==Node.ELEMENT_NODE) {
														Element e5 = (Element)n5;
														String DEF5 = e5.getAttribute("DEF");
														if(l>5) {
														if(DEF5.equals(def[j])) {
															if(l==6) { s = e5.getAttribute(attribute);}
														System.out.println(n5.getNodeName()+" 5");
														NodeList nL5 = n5.getChildNodes();
														nodes = toNodeArr(nL5);
														
												}
												
														}
													}
										}
												}
										}
								}
								}
								}
						}
								}}}}
						}
					}
				}
				
			}
			}
		}//for(Node n : nodes)
	
	//}//for(int i=0;i<path.length;i++)
	}//try
	catch(ParserConfigurationException e) {e.printStackTrace();}
	catch(SAXException e) {e.printStackTrace();}
	catch(IOException e) {e.printStackTrace();}
	return s;
}//decode
public Node[] toNodeArr(NodeList nodeList) {
	Node[] returnNodeArr = new Node[nodeList.getLength()];
	for(int i=0;i<nodeList.getLength();i++) {
		returnNodeArr[i] = nodeList.item(i);
	}
	
	return returnNodeArr;
	
}
}


