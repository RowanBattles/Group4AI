import joblib
import pandas as pd
import os

# Get the absolute path to the current script directory
script_dir = os.path.dirname(os.path.abspath(__file__))

# Load the model
model_path = os.path.join(script_dir, 'svm_model.pkl')
model = joblib.load(model_path)

# Load the scaler
scaler_path = os.path.join(script_dir, 'scaler.pkl')
scaler = joblib.load(scaler_path)

expected_features = [
    'pauses', 'unique_patterns_count', 'total_values_count', 'duplicates',
    'empty_submissions', 'Box_1_Submission', 'Box_2_Submission', 'Box_3_Submission',
    'Box_4_Submission', 'Box_5_Submission', 'Box_6_Submission', 'Box_7_Submission',
    'Box_8_Submission', 'Box_9_Submission', 'Box_10_Submission', 'Box_11_Submission',
    'Box_12_Submission', 'Box_13_Submission', 'Box_14_Submission', 'Box_15_Submission',
    'Box_16_Submission', 'Box_17_Submission', 'Box_18_Submission', 'Box_1_Lines',
    'Box_2_Lines', 'Box_3_Lines', 'Box_4_Lines', 'Box_5_Lines', 'Box_6_Lines',
    'Box_7_Lines', 'Box_8_Lines', 'Box_9_Lines', 'Box_10_Lines', 'Box_11_Lines',
    'Box_12_Lines', 'Box_13_Lines', 'Box_14_Lines', 'Box_15_Lines', 'Box_16_Lines',
    'Box_17_Lines', 'Box_18_Lines', 'Box_1_Clicks', 'Box_2_Clicks', 'Box_3_Clicks',
    'Box_4_Clicks', 'Box_5_Clicks', 'Box_6_Clicks', 'Box_7_Clicks', 'Box_8_Clicks',
    'Box_9_Clicks', 'Box_10_Clicks', 'Box_11_Clicks', 'Box_12_Clicks', 'Box_13_Clicks',
    'Box_14_Clicks', 'Box_15_Clicks', 'Box_16_Clicks', 'Box_17_Clicks', 'Box_18_Clicks',
    'Box_1_Unclicks', 'Box_2_Unclicks', 'Box_3_Unclicks', 'Box_4_Unclicks',
    'Box_5_Unclicks', 'Box_6_Unclicks', 'Box_7_Unclicks', 'Box_8_Unclicks',
    'Box_9_Unclicks', 'Box_10_Unclicks', 'Box_11_Unclicks', 'Box_12_Unclicks',
    'Box_13_Unclicks', 'Box_14_Unclicks', 'Box_15_Unclicks', 'Box_16_Unclicks',
    'Box_17_Unclicks', 'Box_18_Unclicks', 'Box_1_Timegap', 'Box_2_Timegap',
    'Box_3_Timegap', 'Box_4_Timegap', 'Box_5_Timegap', 'Box_6_Timegap', 'Box_7_Timegap',
    'Box_8_Timegap', 'Box_9_Timegap', 'Box_10_Timegap', 'Box_11_Timegap',
    'Box_12_Timegap', 'Box_13_Timegap', 'Box_14_Timegap', 'Box_15_Timegap',
    'Box_16_Timegap', 'Box_17_Timegap', 'Box_18_Timegap'
]

# Read the CSV file passed as an argument
data_path = os.path.join(script_dir, './bin/Debug/test.csv')
data = pd.read_csv(data_path)
data = data[expected_features]

# Scale the data
X_test_scaled = scaler.transform(data)

# predict
y_pred = model.predict(X_test_scaled)

print(y_pred)