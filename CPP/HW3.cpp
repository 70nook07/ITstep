#include <iostream>
#include <climits>
using namespace std;

int main() {
	int choice;
	cout << "Enter 1-3 to choose a task:";
	cin >> choice;
	cout << endl;
	
	
	switch(choice) {
		case 1: {
			const int SIZE = 8;
		    int arr[SIZE];
		    
		    cout << "Enter " << SIZE << " numbers:" << endl;
		    for(int i = 0; i < SIZE; i++) {
		        cout << "Element " << i + 1 << ": ";
		        cin >> arr[i];
		    }
		
		    int sum = 0;
		    int min_element = INT_MAX;
		    int max_element = INT_MIN;
		    
		    for(int i = 0; i < SIZE; i++) {
		        sum += arr[i];
		        
		        if(arr[i] < min_element) {
		            min_element = arr[i];
		        }
		        
		        if(arr[i] > max_element) {
		            max_element = arr[i];
		        }
		    }
		
		    double average = static_cast<double>(sum) / SIZE;
		
		    cout << "\n=== Results ===" << endl;
		    cout << "Array: ";
		    for(int i = 0; i < SIZE; i++) {
		        cout << arr[i] << " ";
		    }
		    cout << "\nSum of all elements: " << sum << endl;
		    cout << "Avarage: " << average << endl;
		    cout << "Min: " << min_element << endl;
		    cout << "Max: " << max_element << endl;
		}
		case 2: {
			int month[6] = {1, 2, 3, 4 ,5 , 6};
			double total_profit = 0;
			
			for (int i : month) {
				cout << "\nEnter profit of " << i << " month. Leaving blank will count as 0: ";
				double profit = 0;
				cin >> profit;
				
				total_profit = total_profit + profit;
			}
			
			cout << "\nTotal profit for 6 months: " << total_profit << endl;

		}
		case 3: {
			int arr[4];
			cout << "\nEnter 4 values for the array.";
			
			for (int i = 0; i < 4; i++) {
			    cin >> arr[i];
			}
			cout << "Your array but reversed: ";
			for (int i = 3; i >= 0; i--) {
				if (i == 0) {
					cout << arr[i] << ".";
					break;
				}
				cout << arr[i] << ", ";
			}
		}
		
	}
    return 0;
}
