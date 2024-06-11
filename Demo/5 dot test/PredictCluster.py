import joblib
import pandas as pd
import os

# Get the absolute path to the current script directory
script_dir = os.path.dirname(os.path.abspath(__file__))

# Load the model
model_path = os.path.join(script_dir, 'svm_model.pkl')
model = joblib.load(model_path)

# Read the CSV file passed as an argument
data_path = os.path.join(script_dir, './bin/Debug/test.csv')
data = pd.read_csv(data_path)

# Assuming your model takes features from the CSV to predict clusters
predictions = model.predict(data)

# Output predictions to the console
for prediction in predictions:
    print(prediction)
