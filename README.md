# ArxmlEditor
Similar to Artop. A Viewer which can view add and delete element in arxml.
![image](https://user-images.githubusercontent.com/101047683/221446727-e58d0556-34fa-4204-9782-086ede521386.png)
* With help of Vector AUTOSAR Scripting Engine, it is easy to handle arxml for all kind of version from R4.0 to R18-11.
* View arxml in a matter of Treeview with expandable style.
* Add child is possible.
* Delete child is also possible.

This repository is a kind of experimental and latter may use this technology in Autosar Configurator repository.

# Usage
## Prepare file
Put the arxml you want to view under the folder of data/bswmd.  

## View
When mouse hover over am item, a short summary will appear.  
![image](https://user-images.githubusercontent.com/101047683/221446867-c281a28f-9ffd-40c7-8e40-e8db14f8f514.png)

Expand treeview can get more child element.  
![image](https://user-images.githubusercontent.com/101047683/221446921-394a45ae-d3de-40f1-9ee7-8338e64cfd8e.png)

## Add
Right click on node can add child element.  
![image](https://user-images.githubusercontent.com/101047683/221446993-c677ee97-2fed-444d-8f4c-ef75ebedd696.png)

## Delete
It is also possilbe to delete element.  
![image](https://user-images.githubusercontent.com/101047683/221447024-51b8a534-f18b-4941-b89a-1a42d01424a4.png)

# Todo
* When add a element with shortname, shortname can be added with a suitable name rather than a blank.
* Edit element.
* Jump to reference.
* Show who reference the node.
