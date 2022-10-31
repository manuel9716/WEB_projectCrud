#include <iostream>

using namespace std;

int main (){
    
    for ( int i = 0; i < 1000; i +=1){
        char respuesta = 'o';
        cout << "deseas terminar el proceso" << endl;
        cin >> respuesta;
        if (respuesta == 'y'){
            cout << "bye bye";
            break;
        }
    }
    
}